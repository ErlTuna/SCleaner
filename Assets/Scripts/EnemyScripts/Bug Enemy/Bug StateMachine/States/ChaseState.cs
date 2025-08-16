using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{   
    Transform target;
    Vector2 targetDirection;
    public ChaseState(GameObject enemy, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent) : base(enemy, player, rb2D, agent){
        target = player.transform;
    }
    public override void OnEnter()
    {
        CalculatePlayerDirection();
        agent.SetDestination(target.position);
    }


    public override void StateUpdate()
    {
        CalculatePlayerDirection();
        agent.SetDestination(target.position);
    }

    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - enemy.transform.position).normalized;
        //rotate towards player
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }

}
