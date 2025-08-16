using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerEnemy : MonoBehaviour, IEnemy
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
            
        //Initialize components
        _stateMachine = new StateMachine();
        _rb2D = GetComponent<Rigidbody2D>();
        _playerInfo = _player.GetComponent<PlayerMain>().playerInfo;
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        //Initialize states
        RoamState roamState = new RoamState(gameObject, _player, _rb2D, agent, SpawnArea);
        ChaseState chaseState = new ChaseState(gameObject, _player, _rb2D, agent);
        ImmobileState immobileState = new ImmobileState(gameObject, _player, _rb2D, agent);
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
        Any(roamState, new FuncPredicate(() => !_playerInfo.isAlive, "player is dead!"));

        #endregion TRANSITIONS

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

    public Coroutine TriggerCoroutine(IEnumerator coroutine){
        return StartCoroutine(coroutine);
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
        EnemyInfo.playerWithinAttackRange = true;
    }

    public void PlayerOutOfAttackRange()
    {  
        EnemyInfo.playerWithinAttackRange = false;
    }

}
