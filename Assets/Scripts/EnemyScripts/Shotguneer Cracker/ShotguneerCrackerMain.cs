using UnityEngine;
using UnityEngine.AI;

public class ShotguneerCrackerMain : Unit
{
    [Header("Unity Components")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] Rigidbody2D _bodyRB;
    [SerializeField] Animator _animator;

    [Header("Visuals")]
    [SerializeField] GameObject _bodyVisuals;
    [SerializeField] EnemyHat _hatScript;

    [Header("Managers")]
    [SerializeField] CrackerHealthManager _healthManager;
    [SerializeField] PlayerDetection _detectionManager;
    [SerializeField] AttackRangeCheck _attackRangeCheck;
    [SerializeField] WeaponHandsManager _weaponHandsManager;
    [SerializeField] EnemyCollisionHandler _collisionHandler;

    [Header("Configs")]
    [SerializeField] UnitAttackConfigSO _attackConfig;
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitMovementConfigSO _movementConfig;
    [SerializeField] EnemyStateConfigSO _stateConfig;
    [SerializeField] EnemyVisualConfigSO _visualConfig;

    [Header("Datas")]
    [SerializeField] UnitHealthData _healthData;
    [SerializeField] EnemyStateData _stateData;

    [Header("Weapon")]
    [SerializeField] GameObject _weaponPrefab;
    GameObject _weaponGO;


    [Header("State Machine & Expected States")]
    [SerializeField] StateMachine _stateMachine;
    RoamState _roamState;
    ImmobileState _immobileState;
    DefeatState _defeatState;
    ShotguneerCrackerAttackState _attackState;

    [Header("Misc")]
    public BoxCollider2D SpawnArea;
    GameObject _player;
    UnitStateData _playerStateData;

    
    void OnEnable()
    {
        _healthManager.OnDefeatContext += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnDefeatContext += HandleDefeat;
    }

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponGO = Instantiate(_weaponPrefab);
        BaseEnemyWeapon baseEnemyWeapon = _weaponGO.GetComponent<BaseEnemyWeapon>();
        if (baseEnemyWeapon)
        {
            baseEnemyWeapon.InitializeWithConfig();
            baseEnemyWeapon.PrepAttackPattern();
        }
        if (_weaponHandsManager)
        {
            _weaponHandsManager.SetWeapon(_weaponGO.transform);
            if (baseEnemyWeapon)
                _weaponHandsManager.SetGripPoints(baseEnemyWeapon.StockGripPoint, baseEnemyWeapon.SecondGripPoint);
        }
            
    }


    void Start()
    {
        // Prep navmesh agent
        if (_agent != null)
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }
        
        if (_player != null)
            _playerStateData = _player.GetComponent<Unit>().GetStateData();

        PrepareRuntimeData();
        InitializeComponents();
        PrepareStateMachine();
        PrepareStateMachineTransitions();

        // Default State
        if (_stateMachine != null && _roamState != null)
            _stateMachine.SetState(_roamState);
    }

    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

    void Update()
    {
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void PrepareRuntimeData()
    {
        _healthData = new UnitHealthData(_healthConfig);
        _stateData = new EnemyStateData(_stateConfig);
    }

    void InitializeComponents()
    {
        _healthManager.InitializeManager(_healthData, _healthConfig, _visualConfig, _hatScript);
        _healthManager.InitializeStateData(_stateData);
        _detectionManager.InitializeStateData(_stateData);
        _attackRangeCheck.InitializeStateData(_stateData);
        _collisionHandler.Initialize(_attackConfig.ContactDamage);
    }

    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject, _agent, SpawnArea, _animator);
        _immobileState = new(gameObject, _rb2D, _agent);
        _defeatState = new(gameObject, _bodyRB, _agent, _bodyVisuals, _visualConfig);
        _attackState = new(gameObject, _weaponHandsManager, _weaponGO, _player, _agent, _stateData, _animator);
    }

    void PrepareStateMachineTransitions()
    {
        //At(_roamState, _chaseState, new FuncPredicate( () => _stateData.HasDetectedPlayer));
        //At(_roamState, _preAttackState, new FuncPredicate(() => _stateData.HasDetectedPlayer));
        //At(roamState, attackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange));
        At(_roamState, _attackState, new FuncPredicate(() => _stateData.PlayerWithinAttackRange));

        //CHASE to STATES
        //At(_chaseState, _roamState, new FuncPredicate( () => _stateData.HasDetectedPlayer == false));
        //At(_chaseState, _preAttackState, new FuncPredicate( () => _stateData.PlayerWithinAttackRange && _stateData.CanMove == true));


        //IMMOBILE to STATES
        //At(_immobileState, _preAttackState, new FuncPredicate( () => _stateData.CanMove && _stateData.PlayerWithinAttackRange));
        //At(_immobileState, _chaseState, new FuncPredicate( () => _stateData.CanMove == true && _stateData.HasDetectedPlayer && _stateData.PlayerWithinAttackRange == false));
        //At(_immobileState, _roamState, new FuncPredicate( () => _stateData.CanMove == true && _stateData.HasDetectedPlayer == false));




        //PRE-ATTACK to STATES
        //At(_preAttackState, _roamState, new FuncPredicate( () => _stateData.IsChargingAnAttack && _stateData.PlayerWithinAttackRange == false && _stateData.HasDetectedPlayer == false));
        //At(_preAttackState, _chaseState, new FuncPredicate( () => _stateData.IsChargingAnAttack && _stateData.PlayerWithinAttackRange == false && _stateData.HasDetectedPlayer));
        //At(_preAttackState, _attackState, new FuncPredicate(() => _stateData.PlayerWithinAttackRange));
        //At(preAttackState, recoveryState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange && !EnemyInfo.isRecovering));

        //ATTACK to STATES
        //At(_attackState, _preAttackState, new FuncPredicate(() => _stateData.HasDetectedPlayer && _stateData.PlayerWithinAttackRange != true));
        //At(_attackState, _postAttackRecoveryState, new FuncPredicate( () => _stateData.HasAttacked));
        //At(attackState, roamState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && !EnemyInfo.hasDetectedPlayer));
        //At(attackState, chaseState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && EnemyInfo.hasDetectedPlayer));        

        //ANY to STATES
        Any(_defeatState, new FuncPredicate(() => _stateData.IsAlive == false));
        //Any(_immobileState, new FuncPredicate( () => _stateData.CanMove == false));
        //Any(roamState, new FuncPredicate(() => !_playerInfo.stateData.isAlive, "player is dead!"));


    }
    
    public void HandleDefeat(DamageContext context)
    {
        _defeatState.SetLastHitContext(context);
        _stateData.IsAlive = false;
    }

    public override UnitStateData GetStateData()
    {
        return _stateData;
    }
}


