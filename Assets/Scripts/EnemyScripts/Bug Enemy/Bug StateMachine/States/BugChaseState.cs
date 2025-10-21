using UnityEngine;
using UnityEngine.AI;

public class BugChaseState : BaseState
{
    Transform _target;
    Vector2 targetDirection;
    public BugChaseState(GameObject owner, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent) : base(owner, rb2D, agent){
        _target = player.transform;
    }
    public override void OnEnter()
    {
        //CalculatePlayerDirection();
        agent.SetDestination(_target.position);
    }


    public override void StateUpdate()
    {
        //CalculatePlayerDirection();
        agent.SetDestination(_target.position);
        if (agent.hasPath)
            RotateTowardsPlayer();

    }

    
    void RotateTowardsPlayer(){

        // subtracting 90f to fix rotation issue as the sprite faces up
        // without this, the enemy rotates 90 degrees more than it should
        Vector2 targetDirection = (_target.position - owner.transform.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -90f;
        owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
