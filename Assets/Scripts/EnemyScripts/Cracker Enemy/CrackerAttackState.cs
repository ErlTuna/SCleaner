using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackState : BaseState
{
    Coroutine attackCoroutine;
    IEnemy enemyScript;
    Transform target;
    Vector2 targetDirection;
    public CrackerAttackState(GameObject enemy, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, IEnemy enemyScript) : base(enemy, player, rb2D, agent){
        target = player.transform;
        this.enemyScript = enemyScript;
        
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
        if(enemyScript.EnemyInfo.isAttacking){
            CalculatePlayerDirection();
            Debug.Log("Launching");
            rb2D.AddForce(targetDirection * 7.5f, ForceMode2D.Impulse);
            enemyScript.EnemyInfo.isAttacking = false;
            enemyScript.TriggerCoroutine(AttackEnd());
        }
            
    }

    IEnumerator AttackEnd(){
        Debug.Log("Attack end!!");
        yield return new WaitForSeconds(.5f);
        enemyScript.EnemyInfo.hasAttacked = true;
    }

    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - enemy.transform.position).normalized;
    }
    void DisableAgent(){
        agent.ResetPath();
        agent.enabled = false;
        rb2D.isKinematic = false;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

}
