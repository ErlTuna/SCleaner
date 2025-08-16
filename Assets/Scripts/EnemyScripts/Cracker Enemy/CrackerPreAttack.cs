using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrackerPreAttack : BaseState
{
    IEnemy enemyScript;
    Transform target;
    Vector2 targetDirection;
    float originalSpeed;
    Coroutine prepareAttack;

    public CrackerPreAttack(GameObject enemy, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, IEnemy enemyScript) : base(enemy, player, rb2D, agent){
        target = player.transform;
        this.enemyScript = enemyScript;
        originalSpeed = agent.speed;
    }

    public override void OnEnter()
    {
        agent.speed *= 0.5f;
        prepareAttack = enemyScript.TriggerCoroutine(PrepareAttack());
    }

    public override void OnExit()
    {
        if(enemyScript.EnemyInfo.isCharging){
            CancelCharge();
        }
        agent.speed = originalSpeed;
    }

    public override void StateUpdate()
    {
        CalculatePlayerDirection();
        RotateTowardsPlayer();
        agent.SetDestination(target.position);
    }

    public override void StateFixedUpdate()
    {
        //no op
    }

    IEnumerator PrepareAttack(){
        enemyScript.EnemyInfo.isCharging = true;
        yield return new WaitForSeconds(.5f);
        Debug.Log("Charged up!");
        enemyScript.EnemyInfo.isCharging = false;
        enemyScript.EnemyInfo.isAttacking = true;
    }

    void CancelCharge(){
        enemyScript.CancelCoroutine(prepareAttack);
        enemyScript.EnemyInfo.isCharging = false;
    }

    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - enemy.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
