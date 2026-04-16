using System;
using UnityEngine;
using UnityEngine.AI;

public class CrackerEnemyMain : Unit, IEnemy
{
    Action OnDefeat;

    [Header("Unity Components")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] Rigidbody2D _bodyRB;
    [SerializeField] Animator _animator;

    [Header("Visuals")]
    [SerializeField] GameObject _body;

    [Header("Managers")]
    [SerializeField] CrackerHealthManager _healthManager;
    [SerializeField] PlayerDetection _detectionManager;
    [SerializeField] AttackRangeCheck _attackRangeCheck;
    [SerializeField] EnemyCollisionHandler _collisionHandler;
    [SerializeField] CurrencyDropper _itemDropper;

    [Header("Configs")]
    [SerializeField] UnitAttackConfigSO _attackConfig;
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitMovementConfigSO _movementConfig;
    [SerializeField] EnemyStateConfigSO _stateConfig;
    [SerializeField] EnemyVisualConfigSO _visualConfig;

    [Header("Datas")]
    UnitHealthData _healthData;
    UnitMovementData _movementData;
    [SerializeField] EnemyStateData _stateData;

    [Header("State Machine & Expected States")]
    StateMachine _stateMachine;
    RoamState _roamState;
    ChaseState _chaseState;
    ImmobileState _immobileState;
    DefeatState _defeatState;
    [SerializeField] CrackerPreAttack _preAttackState;
    CrackerAttackState _attackState;
    CrackerAttackRecoveryState _postAttackRecoveryState;

    [Header("Misc")]
    [SerializeField] BoxCollider2D _spawnArea;
    public BoxCollider2D SpawnArea {get; set;}
    [SerializeField] AfterImageEmitter _afterImageEmitter;
    GameObject _player;
    UnitStateData _playerStateData;


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        _healthManager.OnDefeatWithContext += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnDefeatWithContext -= HandleDefeat;
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
        _movementData = new UnitMovementData(_movementConfig);
        _stateData = new EnemyStateData(_stateConfig);
    }

    void InitializeComponents()
    {
        _healthManager.InitializeManager(_healthData, _healthConfig, _visualConfig);
        _healthManager.InitializeStateData(_stateData);
        _detectionManager.InitializeStateData(_stateData);
        _attackRangeCheck.InitializeStateData(_stateData);
        _collisionHandler.Initialize(_attackConfig.ContactDamage);
    }



    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject,_agent, _spawnArea, _animator);
        _chaseState = new(gameObject, _player, _agent, _animator);
        _immobileState = new(gameObject, _rb2D, _agent);
        _defeatState = new(gameObject, _bodyRB, _agent, _body, _visualConfig);
        _preAttackState = new(gameObject, this, _player, _rb2D, _agent, _stateData, _movementData);
        _attackState = new(gameObject, this, _player, _rb2D, _agent, _stateData, _afterImageEmitter);
        _postAttackRecoveryState = new(gameObject, this, _player, _rb2D, _agent, _stateData);
    }

    void PrepareStateMachineTransitions()
    {
        At(_roamState, _chaseState, new FuncPredicate(() => _stateData.HasDetectedPlayer));
        At(_roamState, _preAttackState, new FuncPredicate(() => _stateData.PlayerWithinAttackRange));
        //At(roamState, attackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange));

        //CHASE to STATES
        At(_chaseState, _roamState, new FuncPredicate(() => _stateData.HasDetectedPlayer == false));
        At(_chaseState, _preAttackState, new FuncPredicate(() => _stateData.PlayerWithinAttackRange && _stateData.CanMove == true));


        //IMMOBILE to STATES
        At(_immobileState, _preAttackState, new FuncPredicate(() => _stateData.CanMove && _stateData.PlayerWithinAttackRange));
        At(_immobileState, _chaseState, new FuncPredicate(() => _stateData.CanMove == true && _stateData.HasDetectedPlayer && _stateData.PlayerWithinAttackRange == false));
        At(_immobileState, _roamState, new FuncPredicate(() => _stateData.CanMove == true && _stateData.HasDetectedPlayer == false));

        //PRE-ATTACK to STATES
        At(_preAttackState, _roamState, new FuncPredicate(() => _stateData.IsChargingAnAttack && _stateData.PlayerWithinAttackRange == false && _stateData.HasDetectedPlayer == false));
        At(_preAttackState, _chaseState, new FuncPredicate(() => _stateData.IsChargingAnAttack && _stateData.PlayerWithinAttackRange == false && _stateData.HasDetectedPlayer));
        At(_preAttackState, _attackState, new FuncPredicate(() => _stateData.IsAttacking));
        //At(preAttackState, recoveryState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange && !EnemyInfo.isRecovering));

        //ATTACK to STATES
        At(_attackState, _postAttackRecoveryState, new FuncPredicate(() => _stateData.HasAttacked));
        //At(attackState, roamState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && !EnemyInfo.hasDetectedPlayer));
        //At(attackState, chaseState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && EnemyInfo.hasDetectedPlayer));

        //RECOVERY to STATES
        At(_postAttackRecoveryState, _roamState, new FuncPredicate(() => _stateData.IsRecoveringPostAttack == false && _stateData.HasDetectedPlayer == false));
        At(_postAttackRecoveryState, _chaseState, new FuncPredicate(() => _stateData.IsRecoveringPostAttack == false && _stateData.HasDetectedPlayer));
        At(_postAttackRecoveryState, _preAttackState, new FuncPredicate(() => _stateData.IsRecoveringPostAttack == false && _stateData.PlayerWithinAttackRange));



        //ANY to STATES
        Any(_immobileState, new FuncPredicate(() => _stateData.CanMove == false));
        Any(_defeatState, new FuncPredicate(() => _stateData.IsAlive == false));
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

    public void AssignSpawnArea(BoxCollider2D spawnArea)
    {
        _spawnArea = spawnArea;
    }

    public void AssignOnDefeatCallback(Action onDefeat)
    {
        Debug.Log("Assigned on defeat callback.");
        OnDefeat = onDefeat;
    }
}
