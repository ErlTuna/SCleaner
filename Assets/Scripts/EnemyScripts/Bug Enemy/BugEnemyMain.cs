
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class BugEnemyMain : Unit
{

    [Header("Unity Components")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] Rigidbody2D _bodyRB;


    [Header("Visuals")]
    [SerializeField] GameObject _visuals;

    [Header("Managers")]
    [SerializeField] BugHealthManager _healthManager;
    [SerializeField] PlayerDetection _detectionManager;
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
    

    
    [Header("State Machine & Expected States")]
    StateMachine _stateMachine;
    BugRoamState _roamState;
    BugChaseState _chaseState;
    ImmobileState _immobileState;
    DefeatState _defeatState;

    [Header("Misc")]
    public BoxCollider2D SpawnArea;
    GameObject _player;
    UnitStateData _playerStateData;

    void OnEnable()
    {
        _healthManager.OnDefeat += HandleDefeat;
    }

    void OnDisable()
    {
        _healthManager.OnDefeat += HandleDefeat;
    }

    void Awake()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Start()
    {
        // Prep navmesh agent
        if (_agent != null)
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

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

    public void HandleDefeat(DamageContext context)
    {
        _defeatState.SetLastHitContext(context);
        _stateData.IsAlive = false;
    }

        void PrepareRuntimeData()
    {
        _healthData = new UnitHealthData(_healthConfig);
        _stateData = new EnemyStateData(_stateConfig);
    }

    void InitializeComponents()
    {
        _healthManager.InitializeManager(_healthData, _healthConfig, _visualConfig);
        _healthManager.InitializeStateData(_stateData);
        _detectionManager.InitializeStateData(_stateData);
        _collisionHandler.Initialize(_attackConfig.ContactDamage);
    }

    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject, _agent, SpawnArea);
        _chaseState = new(gameObject, _player, _agent);
        _immobileState = new(gameObject, _rb2D, _agent);
        _defeatState = new(gameObject, _bodyRB, _agent, _visuals, _visualConfig);
    }

    void PrepareStateMachineTransitions()
    {
        //ROAM to STATES
        At(_roamState, _chaseState, new FuncPredicate(() => _stateData.HasDetectedPlayer));


        //CHASE to STATES
        At(_chaseState, _roamState, new FuncPredicate(() => _stateData.HasDetectedPlayer == false));

        //IMMOBILE to STATES
        At(_immobileState, _roamState, new FuncPredicate(() => _stateData.CanMove && _stateData.HasDetectedPlayer == false));
        At(_immobileState, _chaseState, new FuncPredicate(() => _stateData.CanMove && _stateData.HasDetectedPlayer));

        //ANY TRANSITIONS
        Any(_defeatState, new FuncPredicate(() => _stateData.IsAlive == false));
        Any(_immobileState, new FuncPredicate(() => _stateData.CanMove == false && _stateData.IsAlive));
        Any(_roamState, new FuncPredicate(() => !_playerStateData.IsAlive && _stateData.CanMove));
    }

    public override UnitStateData GetStateData()
    {
        return _stateData;
    }
}
