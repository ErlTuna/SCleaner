using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedCrackerMain : Unit
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] CrackerHealthManager _healthManager;
    [SerializeField] PlayerDetection _detectionManager;
    [SerializeField] AttackRangeCheck _attackRangeCheck;
    [SerializeField] GameObject _visuals;
    [SerializeField] GameObject _weaponPrefab;
    [SerializeField] WeaponHandsManager _weaponHandsManager;
    GameObject _weaponGO;
    EnemyStateData _stateData;
    UnitMovementData _movementData;
    public BoxCollider2D SpawnArea;
    StateMachine _stateMachine;
    GameObject _player;
    UnitStateData _playerStateData;

    // Expected states
    RoamState _roamState;
    ImmobileState _immobileState;
    DefeatState _defeatState;
    RangedCrackerAttackState _attackState;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponGO = Instantiate(_weaponPrefab);
        _weaponGO.GetComponent<BaseEnemyWeapon>().InitializeWithConfig();
        _weaponHandsManager.SetWeapon(_weaponGO.transform);
        InitializeRuntimeData();
        PrepareStateMachine();
    }

    void OnEnable()
    {
        _healthManager.OnDefeat += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnDefeat += HandleDefeat;
    }

    void Start()
    {
        // Prep navmesh agent
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        if (_player != null)
            _playerStateData = _player.GetComponent<Unit>().GetRuntimeData<UnitStateData>();

        PrepareStateMachineTransitions();
        InitializeComponents();

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

    void InitializeRuntimeData()
    {
        RuntimeDataHolder = new UnitRuntimeDataHolder();

        RuntimeDataHolder.AddRuntimeData(new UnitHealthData());
        RuntimeDataHolder.AddRuntimeData(new UnitMovementData());
        RuntimeDataHolder.AddRuntimeData(new EnemyStateData());

        RuntimeDataHolder?.InitializeWithWrapper(this, UnitConfigWrapper);

        // Caching as it is accessed frequently 
        _stateData = RuntimeDataHolder.GetRuntimeData<EnemyStateData>();
        _movementData = RuntimeDataHolder.GetRuntimeData<UnitMovementData>();
    }

    void InitializeComponents()
    {
        _detectionManager.Initialize(this);
        _healthManager.Initialize(this);
    }

    public void HandleDefeat(DamageContext context)
    {
        _defeatState.SetLastHitContext(context);
        _stateData.IsAlive = false;
    }

    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject, _rb2D, agent, SpawnArea);
        //_chaseState = new(gameObject, _player, _rb2D, agent);
        //_immobileState = new(gameObject, _rb2D, agent);
        _defeatState = new(gameObject, _rb2D, agent, _visuals, UnitConfigWrapper.defeatSprite);
        //_preAttackState = new(gameObject, this, _weaponHandsManager, _player, _rb2D, agent, _stateData);
        _attackState = new(gameObject, this, _weaponHandsManager, _weaponGO, _player, _rb2D, agent, _stateData, UnitConfigWrapper.attackConfigSO);
        //_postAttackRecoveryState = new(gameObject, this, _player, _rb2D, agent, _stateData);
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
}
