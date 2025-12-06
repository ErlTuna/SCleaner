using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Transform _target;
    Animator _animator;
    Vector2 _targetDirection;

    public ChaseState(GameObject owner, GameObject player, NavMeshAgent agent, Animator animator = null)
    {
        _owner = owner;
        _target = player.transform;
        _agent = agent;
        _animator = animator;
    }
    public override void OnEnter()
    {
        //CalculatePlayerDirection();
        _owner.transform.rotation = Quaternion.identity;
        _agent.SetDestination(_target.position);
        _animator.SetBool("isMoving", true);
    }


    public override void StateUpdate()
    {
        //CalculatePlayerDirection();
        _agent.SetDestination(_target.position);
    }

    void CalculatePlayerDirection(){
        //calculate direction
        _targetDirection = (_target.position - _owner.transform.position).normalized;
        //rotate towards player
        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
        _owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public override void OnExit()
    {
        _animator.SetBool("isMoving", false);
    }


}
