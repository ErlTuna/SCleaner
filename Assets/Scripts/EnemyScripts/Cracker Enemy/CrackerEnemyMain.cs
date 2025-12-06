using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerEnemyMain : Unit
{
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
    CrackerPreAttack _preAttackState;
    CrackerAttackState _attackState;
    CrackerAttackRecoveryState _postAttackRecoveryState;

    [Header("Misc")]
    public BoxCollider2D SpawnArea;
    [SerializeField] AfterImageEmitter _afterImageEmitter;
    GameObject _player;
    UnitStateData _playerStateData;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        _healthManager.OnDefeatContext += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnDefeatContext += HandleDefeat;
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

    /*
    void Start()
    {

        //Initialize components
        _stateMachine = new StateMachine();
        _rb2D = GetComponent<Rigidbody2D>();
        //_playerInfo = _player.GetComponent<PlayerMain>().playerInfo;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        

        //Initialize states
        RoamState roamState = new RoamState(gameObject, _rb2D, agent, SpawnArea);
        ChaseState chaseState = new ChaseState(gameObject, _player, _rb2D, agent);
        ImmobileState immobileState = new ImmobileState(gameObject, _rb2D, agent);
        CrackerAttackState attackState = new CrackerAttackState(gameObject, _player, _rb2D, agent, this);
        CrackerAttackRecoveryState recoveryState = new CrackerAttackRecoveryState(gameObject, _player, _rb2D, agent, this);
        CrackerPreAttack preAttackState = new CrackerPreAttack(gameObject, _player, _rb2D, agent, this);

        //Initialize transitions
        #region TRANSITIONS
        //ROAM to STATES
        At(roamState, chaseState, new FuncPredicate( () => EnemyInfo.hasDetectedPlayer));
        At(roamState, preAttackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange));
        //At(roamState, attackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange));

        //CHASE to STATES
        At(chaseState, roamState, new FuncPredicate( () => !EnemyInfo.hasDetectedPlayer));
        At(chaseState, preAttackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange && !EnemyInfo.isImmobilized));
        

        //IMMOBILE to STATES
        At(immobileState, preAttackState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange && !EnemyInfo.isImmobilized));
        At(immobileState, chaseState, new FuncPredicate( () => !EnemyInfo.isImmobilized && EnemyInfo.hasDetectedPlayer && !EnemyInfo.playerWithinAttackRange));
        At(immobileState, roamState, new FuncPredicate( () => !EnemyInfo.isImmobilized && !EnemyInfo.hasDetectedPlayer));
        
        //PRE-ATTACK to STATES
        At(preAttackState, roamState, new FuncPredicate( () => EnemyInfo.isCharging && !EnemyInfo.playerWithinAttackRange && !EnemyInfo.hasDetectedPlayer));
        At(preAttackState, chaseState, new FuncPredicate( () => EnemyInfo.isCharging && !EnemyInfo.playerWithinAttackRange && EnemyInfo.hasDetectedPlayer));
        At(preAttackState, attackState, new FuncPredicate( () => EnemyInfo.isAttacking));
        //At(preAttackState, recoveryState, new FuncPredicate( () => EnemyInfo.playerWithinAttackRange && !EnemyInfo.isRecovering));

        //ATTACK to STATES
        At(attackState, recoveryState, new FuncPredicate( () => EnemyInfo.hasAttacked));
        //At(attackState, roamState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && !EnemyInfo.hasDetectedPlayer));
        //At(attackState, chaseState, new FuncPredicate( () => !EnemyInfo.playerWithinAttackRange && EnemyInfo.hasDetectedPlayer));

        //RECOVERY to STATES
        At(recoveryState, roamState, new FuncPredicate( () => !EnemyInfo.isRecovering && !EnemyInfo.hasDetectedPlayer));
        At(recoveryState, chaseState, new FuncPredicate( () => !EnemyInfo.isRecovering && EnemyInfo.hasDetectedPlayer));
        At(recoveryState, preAttackState, new FuncPredicate( () => !EnemyInfo.isRecovering && EnemyInfo.playerWithinAttackRange));
        
        

        //ANY to STATES
        Any(immobileState, new FuncPredicate( () => EnemyInfo.isImmobilized));
        //Any(roamState, new FuncPredicate(() => !_playerInfo.stateData.isAlive, "player is dead!"));

        #endregion TRANSITIONS

        _stateMachine.SetState(roamState);

        
    }

    */

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
        _roamState = new(gameObject,_agent, SpawnArea, _animator);
        _chaseState = new(gameObject, _player, _agent, _animator);
        _immobileState = new(gameObject, _rb2D, _agent);
        _defeatState = new(gameObject, _bodyRB, _agent, _body, _visualConfig);
        _preAttackState = new(gameObject, this, _player, _rb2D, _agent, _stateData, _movementData);
        _attackState = new(gameObject, this, _player, _rb2D, _agent, _stateData, _afterImageEmitter);
        _postAttackRecoveryState = new(gameObject, this, _player, _rb2D, _agent, _stateData);
    }

    /*
    void PrepareStateMachineTransitions()
    {
        //ROAM to STATES
        At(_roamState, _chaseState, new FuncPredicate(() => _stateData.HasDetectedPlayer));


        //CHASE to STATES
        At(_chaseState, _roamState, new FuncPredicate(() => !_stateData.HasDetectedPlayer));

        //IMMOBILE to STATES
        At(_immobileState, _roamState, new FuncPredicate(() => _stateData.CanMove && !_stateData.HasDetectedPlayer));
        At(_immobileState, _chaseState, new FuncPredicate(() => _stateData.CanMove && _stateData.HasDetectedPlayer));

        //ANY TRANSITIONS
        Any(_defeatState, new FuncPredicate(() => !_stateData.IsAlive));
        Any(_immobileState, new FuncPredicate(() => _stateData.CanMove == false && _stateData.IsAlive));
        Any(_roamState, new FuncPredicate(() => !_playerStateData.IsAlive && _stateData.CanMove));
    }
    */

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
