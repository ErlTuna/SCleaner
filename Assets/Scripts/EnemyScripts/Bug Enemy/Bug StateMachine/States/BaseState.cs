using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState : IState
{
    protected readonly GameObject owner;
    protected readonly GameObject player;
    protected readonly Rigidbody2D rb2D;
    protected readonly NavMeshAgent agent; 

    public BaseState(GameObject owner, Rigidbody2D rb2D, NavMeshAgent agent){
        this.owner = owner;
        this.rb2D = rb2D;
        this.agent = agent;
    }


    public virtual void OnEnter()
    {
        //noop
    }

    public virtual void OnExit()
    {
        //noop
    }

    public virtual void StateFixedUpdate()
    {
        //noop
    }

    public virtual void StateUpdate()
    {
        //noop
    }

    public virtual void TriggerCoroutine(Coroutine coroutine){
        
    }

}
