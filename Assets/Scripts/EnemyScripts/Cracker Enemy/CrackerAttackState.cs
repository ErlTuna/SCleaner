using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackState : BaseState
{
    Coroutine attackCoroutine;
    Unit _ownerScript;
    Transform target;
    Vector2 targetDirection;
    EnemyStateData _stateData;
    public CrackerAttackState(GameObject owner, Unit ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData, UnitMovementData movementData) : base(owner, rb2D, agent)
    {
        target = player.transform;
        _ownerScript = ownerScript;
        _stateData = stateData;
    }
    public override void OnEnter()
    {
        Debug.Log("Entered Cracker Attack");
        //agent.ResetPath();
        //rb2D.isKinematic = false;
        //rb2D.bodyType = RigidbodyType2D.Dynamic;
        //agent.enabled = false;
        DisableAgent();
        
    }

    public override void OnExit()
    {
        Debug.Log("Exited Cracker Attack");
        //rb2D.velocity = Vector2.zero;
        //rb2D.angularVelocity = 0f;
    }

    public override void StateUpdate()
    {
        //no op
    }

    public override void StateFixedUpdate()
    {
        /*if(!enemyScript.EnemyInfo.isAttacking){
            attackCoroutine = enemyScript.TriggerCoroutine(PerformAttack());
        }*/
        if(_stateData.IsAttacking){
            CalculatePlayerDirection();
            Debug.Log("Launching");
            rb2D.AddForce(targetDirection * 7.5f, ForceMode2D.Impulse);
            
            _ownerScript.StartCoroutine(AttackEnd());
        }
            
    }

    IEnumerator AttackEnd()
    {
        _stateData.IsAttacking = false;
        Debug.Log("Attack end!!");
        yield return new WaitForSeconds(1.5f);
        _stateData.HasAttacked = true;
    }

    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - owner.transform.position).normalized;
    }
    void DisableAgent(){
        agent.ResetPath();
        agent.enabled = false;
        rb2D.isKinematic = false;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

}
