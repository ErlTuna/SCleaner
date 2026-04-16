using System;
using UnityEngine;
using UnityEngine.AI;

public class ShotguneerCrackerMain : Unit, IEnemy
{
    Action OnDefeat;

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
    [SerializeField] CurrencyDropper _itemDropper;

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
    [SerializeField] AttackPatternSO _attackPattern;
    //public AttackPatternSO AttackPatern => _attackPattern;


    [Header("Misc")]
    [SerializeField] BoxCollider2D _spawnArea;
    public BoxCollider2D SpawnArea {get; set;}
    GameObject _player;
    UnitStateData _playerStateData;
    float _losTimer = 0.5f;
    [SerializeField] LayerMask _obstructionMask;

    
    void OnEnable()
    {
        _healthManager.OnHitByPlayer += SetProvoked;
        _healthManager.OnDefeatWithContext += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnHitByPlayer -= SetProvoked;
        _healthManager.OnDefeatWithContext -= HandleDefeat;
    }

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponGO = Instantiate(_weaponPrefab);
        BaseEnemyWeapon baseEnemyWeapon = _weaponGO.GetComponent<BaseEnemyWeapon>();
        if (baseEnemyWeapon)
        {
            //baseEnemyWeapon.InitializeUsingOwnConfig();
            //baseEnemyWeapon.PrepAttackPattern();
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
        UpdatePerception();
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

        _itemDropper.Initialize(_healthManager, _player.transform);
    }

    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject, _agent, _spawnArea, _animator);
        _immobileState = new(gameObject, _rb2D, _agent);
        _defeatState = new(gameObject, _bodyRB, _agent, _bodyVisuals, _visualConfig);
        _attackState = new(gameObject, _weaponHandsManager, _weaponGO, _player, _agent, _stateData, _animator, Instantiate(_attackPattern));
    }

    void PrepareStateMachineTransitions()
    {
        //At(_roamState, _chaseState, new FuncPredicate( () => _stateData.HasDetectedPlayer));
        //At(_roamState, _preAttackState, new FuncPredicate(() => _stateData.HasDetectedPlayer));
        //At(roamState, attackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange));
        At(_roamState, _attackState, new FuncPredicate(() => _stateData.PlayerWithinAttackRange));
        At(_roamState, _attackState, new FuncPredicate(() => _stateData.HasBeenAttacked && _stateData.CanMove == true));

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
        //At(_attackState, _roamState, new FuncPredicate( () => !_stateData.PlayerWithinAttackRange && !_stateData.HasDetectedPlayer));
        //At(attackState, chaseState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && EnemyInfo.hasDetectedPlayer));        

        //ANY to STATES
        Any(_defeatState, new FuncPredicate(() => _stateData.IsAlive == false));
        //Any(_immobileState, new FuncPredicate( () => _stateData.CanMove == false));
        Any(_roamState, new FuncPredicate(() => _playerStateData.IsAlive == false, "player is dead!"));


    }
    
    void HandleDefeat(DamageContext context)
    {
        Debug.Log("Handle defeat called.");
        _defeatState.SetLastHitContext(context);
        _stateData.IsAlive = false;
        if (_defeatEventChannel != null)
            _defeatEventChannel.RaiseEvent();

        OnDefeat?.Invoke();
    }

    public override UnitStateData GetStateData()
    {
        return _stateData;
    }

    void SetProvoked()
    {
        Debug.Log("PROVOKED!");
        _stateData.HasBeenAttacked = true;
    }

    bool HasLineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 dir = (_player.transform.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, _player.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, _obstructionMask);

        return hit.collider == null;
    }

    void UpdatePerception()
    {
        if (_stateData.PlayerWithinAttackRange)
        {
            _losTimer -= Time.deltaTime;
            if (_losTimer <= 0f)
            {
                _stateData.HasLineOfSight = HasLineOfSight();
                if (_stateData.HasLineOfSight == false)
                    Debug.Log("NO LOS!");
                else 
                    Debug.Log("LOS!");
                _losTimer = 0.5f;
            }
        }
        else
        {
            // Out of range, no need for LoS checks
            _stateData.HasLineOfSight = false;
        }
    }

    public void AssignSpawnArea(BoxCollider2D spawnArea)
    {
        _spawnArea = spawnArea;
    }

    public void AssignOnDefeatCallback(Action onDefeat)
    {
        OnDefeat = onDefeat;
    }
}


