using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostlingEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody2D _rb2D;
    public EnemySO EnemyInfo{get;set;}
    public BoxCollider2D SpawnArea {get;set;}
    public UnitInfoSO _playerInfo;
    StateMachine _stateMachine;
    GameObject _player;
    
    
    
    void Awake(){
        _player = GameObject.FindGameObjectWithTag("Player");
        if (EnemyInfo == null){
            EnemyInfo = ScriptableObject.CreateInstance<EnemySO>();
        }
    EnemyInfo.Init();
    }        

    void Start()
    {
        

        //Initialize fields
        //string description1 = "roam to stop predicate";
        //string description2 = "stop to roam predicate";
        


        //Initialize components
        _stateMachine = new StateMachine();
        _rb2D = GetComponent<Rigidbody2D>();
        _playerInfo = _player.GetComponent<PlayerMain>().playerInfo;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //agent.updatePosition = true;


        //Initialize states
        RoamState roamState = new RoamState(gameObject, _player, _rb2D, agent, SpawnArea);
        ChaseState chaseState = new ChaseState(gameObject, _player, _rb2D, agent);
        ImmobileState immobileState = new ImmobileState(gameObject, _player, _rb2D, agent);
        
        //Initialize transitions
        At(roamState, chaseState, new FuncPredicate( () => EnemyInfo.hasDetectedPlayer));
        At(chaseState, roamState, new FuncPredicate( () => !EnemyInfo.hasDetectedPlayer));
        At(immobileState, chaseState, new FuncPredicate( () => !EnemyInfo.isImmobilized && EnemyInfo.hasDetectedPlayer));
        At(immobileState, chaseState, new FuncPredicate( () => !EnemyInfo.isImmobilized && !EnemyInfo.hasDetectedPlayer));

        Any(immobileState, new FuncPredicate( () => EnemyInfo.isImmobilized));


        //At(roamState, chaseState, new FuncPredicate( () => infoSO.hasDetectedPlayer));

        Any(roamState, new FuncPredicate(() => !_playerInfo.isAlive, "player is dead!"));


        //Set initial state
        _stateMachine.SetState(roamState);
    }

    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

    void Update(){
        _stateMachine.Update();
    }

    void FixedUpdate(){
        _stateMachine.FixedUpdate();
    }

    public void InvokeCoroutine(IEnumerator coroutine){
        StartCoroutine(coroutine);
    }

    public void CancelCoroutine(Coroutine corutine){
        StopCoroutine(corutine);
    }

    public void PlayerIsDetected(){
        EnemyInfo.hasDetectedPlayer = true; 
    }
    public void PlayerOutOfRange(){
        EnemyInfo.hasDetectedPlayer = false;
    }
    public void SetProvoked()
    {
        EnemyInfo.hasBeenAttacked = true;
    }
    public void Attack(){
        //no op
    }

    public void PlayerWithinAttackRange()
    {
        //no op
    }

    public void PlayerOutOfAttackRange()
    {
        //no op
    }

    public Coroutine TriggerCoroutine(IEnumerator coroutine)
    {
        throw new NotImplementedException();
    }
}
