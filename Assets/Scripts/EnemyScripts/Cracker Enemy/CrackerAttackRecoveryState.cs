using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackRecoveryState : BaseState
{
    Coroutine recoveryRoutine;
    IEnemy enemyScript;
    Vector2 targetDirection;
    Transform target;

    public CrackerAttackRecoveryState(GameObject enemy, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, IEnemy enemyScript) : base(enemy, player, rb2D, agent){
        this.enemyScript = enemyScript;
        target = player.transform;
    }
    public override void OnEnter()
    {
        Debug.Log("entered recovery");
        enemyScript.EnemyInfo.hasAttacked = false;
        recoveryRoutine = enemyScript.TriggerCoroutine(PostAttackRecovery());
    }

    public override void OnExit()
    {
        if(enemyScript.EnemyInfo.isImmobilized){
            if(recoveryRoutine != null)
                enemyScript.CancelCoroutine(recoveryRoutine);
        }
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f;
        EnableAgent();
    }

    public override void StateFixedUpdate()
    {
        //noop
    }

    public override void StateUpdate()
    {
        //CalculatePlayerDirection();
        //RotateTowardsPlayer();
    }
    IEnumerator PostAttackRecovery(){
        enemyScript.EnemyInfo.isRecovering = true;
        yield return new WaitForSeconds(1.5f);
        enemyScript.EnemyInfo.isRecovering = false;
    }
    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - enemy.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void EnableAgent(){
        agent.enabled = true;
        rb2D.isKinematic = true;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
