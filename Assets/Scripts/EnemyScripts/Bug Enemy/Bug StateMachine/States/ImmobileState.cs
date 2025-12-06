using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Interactions;

public class ImmobileState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Rigidbody2D _rb2D;

    public ImmobileState(GameObject owner, Rigidbody2D rb2D, NavMeshAgent agent)
    {
        _owner = owner;
        _rb2D = rb2D;
        _agent = agent;
    }
    public override void OnEnter()
    {
        if (_agent.hasPath)
            _agent.ResetPath();

        _agent.enabled = false;
        _rb2D.isKinematic = false;
        Debug.Log("Immobilized!");
    }

    public override void OnExit()
    {
        _rb2D.velocity = Vector2.zero;
        _rb2D.angularVelocity = 0f;
        _rb2D.isKinematic = true;
        _agent.enabled = true;
        Debug.Log("Exited immobile state");
    }


}
