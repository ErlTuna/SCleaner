using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Interactions;

public class ImmobileState : BaseState
{
    public ImmobileState(GameObject enemy, Rigidbody2D rb2D, NavMeshAgent agent) : base(enemy, rb2D, agent){}
    public override void OnEnter()
    {
        if (agent.hasPath)
            agent.ResetPath();

        agent.enabled = false;
        rb2D.isKinematic = false;
        //rb2D.bodyType = RigidbodyType2D.Dynamic;

        Debug.Log("Immobilized!");
    }

    public override void OnExit()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f;
        rb2D.isKinematic = true;
        agent.enabled = true;
        Debug.Log("Exited immobile state");
        //rb2D.bodyType = RigidbodyType2D.Kinematic;

    }


}
