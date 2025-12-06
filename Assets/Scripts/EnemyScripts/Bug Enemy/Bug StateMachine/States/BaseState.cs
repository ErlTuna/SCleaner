using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState : IState
{

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

}
