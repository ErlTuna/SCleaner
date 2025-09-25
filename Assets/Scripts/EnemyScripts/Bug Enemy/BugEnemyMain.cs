
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
    public EnemyStateData stateData;
    public BoxCollider2D SpawnArea;
    StateMachine _stateMachine;
    GameObject _player;
    UnitStateData playerStateData;

    // Expected states
    RoamState _roamState;
    ChaseState _chaseState;
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

        playerStateData = _player.GetComponent<Unit>().GetRuntimeData<UnitStateData>();
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
        stateData.IsAlive = false;
    }

    void InitializeRuntimeData()
    {
        RuntimeDataHolder = new UnitRuntimeDataHolder();

        RuntimeDataHolder.AddRuntimeData(new UnitHealthData());
        RuntimeDataHolder.AddRuntimeData(new UnitMovementData());
        RuntimeDataHolder.AddRuntimeData(new EnemyStateData());

        RuntimeDataHolder?.InitializeWithWrapper(this, UnitConfigWrapper);

        // Caching as it is accessed frequently 
        stateData = RuntimeDataHolder.GetRuntimeData<EnemyStateData>();
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
        At(_roamState, _chaseState, new FuncPredicate(() => stateData.HasDetectedPlayer));


        //CHASE to STATES
        At(_chaseState, _roamState, new FuncPredicate(() => !stateData.HasDetectedPlayer));

        //IMMOBILE to STATES
        At(_immobileState, _roamState, new FuncPredicate(() => stateData.CanMove && !stateData.HasDetectedPlayer));
        At(_immobileState, _chaseState, new FuncPredicate(() => stateData.CanMove && stateData.HasDetectedPlayer));

        //ANY TRANSITIONS
        Any(_defeatState, new FuncPredicate(() => !stateData.IsAlive));
        Any(_immobileState, new FuncPredicate(() => stateData.CanMove == false && stateData.IsAlive));
        Any(_roamState, new FuncPredicate(() => !playerStateData.IsAlive && stateData.CanMove));
    }

    void InitializeComponents()
    {
        _detectionManager.Initialize(this);
        _healthManager.Initialize(this);
    }
}
