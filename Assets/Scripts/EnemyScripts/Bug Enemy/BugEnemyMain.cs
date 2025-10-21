
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class BugEnemyMain : Unit
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] BugHealth_v2 _healthManager;
    [SerializeField] EnemyMovementManager _movementManager;
    [SerializeField] PlayerDetection _detectionManager;
    [SerializeField] GameObject _visuals;
    EnemyStateData _stateData;
    public BoxCollider2D SpawnArea;
    StateMachine _stateMachine;
    GameObject _player;
    UnitStateData _playerStateData;

    // Expected states
    BugRoamState _roamState;
    BugChaseState _chaseState;
    ImmobileState _immobileState;
    DefeatState _defeatState;


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
        if (UnitConfigWrapper == null)
        {
            Debug.Log("Unit config is null");
        }

        InitializeRuntimeData();

        _player = GameObject.FindGameObjectWithTag("Player");
        
        PrepareStateMachine();
    }

    void Start()
    {
        // Prep navmesh agent
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

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

    public void HandleDefeat(DamageContext context)
    {

        _defeatState.SetLastHitContext(context);
        _stateData.IsAlive = false;
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
    }

    void PrepareStateMachine()
    {
        //Initialize states
        _stateMachine = new StateMachine();
        _roamState = new(gameObject, _rb2D, agent, SpawnArea);
        _chaseState = new(gameObject, _player, _rb2D, agent);
        _immobileState = new(gameObject, _rb2D, agent);
        _defeatState = new(gameObject, _rb2D, agent, _visuals);
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
        Any(_defeatState, new FuncPredicate(() => !_stateData.IsAlive));
        Any(_immobileState, new FuncPredicate(() => _stateData.CanMove == false && _stateData.IsAlive));
        Any(_roamState, new FuncPredicate(() => !_playerStateData.IsAlive && _stateData.CanMove));
    }

    void InitializeComponents()
    {
        _detectionManager.Initialize(this);
        _healthManager.Initialize(this);
    }
}
