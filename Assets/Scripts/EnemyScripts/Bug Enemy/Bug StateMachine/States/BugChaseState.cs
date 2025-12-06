using UnityEngine;
using UnityEngine.AI;

public class BugChaseState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Transform _target;
    public BugChaseState(GameObject owner, GameObject player, NavMeshAgent agent)
    {
        _owner = owner;
        _target = player.transform;
        _agent = agent;
        
    }
    public override void OnEnter()
    {
        //CalculatePlayerDirection();
        if(_agent.enabled)
            _agent.SetDestination(_target.position);
    }


    public override void StateUpdate()
    {
        //CalculatePlayerDirection();
        if(_agent && _agent.enabled)
            _agent.SetDestination(_target.position);
        if (_agent.hasPath)
            RotateTowardsPlayer();

    }

    
    void RotateTowardsPlayer(){

        // subtracting 90f to fix rotation issue as the sprite faces up
        // without this, the enemy rotates 90 degrees more than it should
        Vector2 targetDirection = (_target.position - _owner.transform.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -90f;
        _owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
