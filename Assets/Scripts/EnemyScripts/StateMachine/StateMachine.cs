using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();

    public void Update() {
        ITransition transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }

        current.State?.StateUpdate();
    }

    public void FixedUpdate(){
        current.State?.StateFixedUpdate();
    }

    public void SetState(IState state){
        current = nodes[state.GetType()];
        current.State?.OnEnter();
    }

    void ChangeState(IState state) {
        if (current.State == state) return;

        IState previousState = current.State;
        IState nextState = nodes[state.GetType()].State;
        
        previousState.OnExit();
        nextState?.OnEnter();
        current = nodes[state.GetType()];
    }

    ITransition GetTransition() {
        foreach (ITransition transition in anyTransitions){
            if(transition.Condition.Evaluate()){
                //Debug.Log("Found any transition : " + transition);
                return transition;
            }
        }

        foreach (ITransition transition in current.Transitions){
            if(transition.Condition.Evaluate()){
                //Debug.Log("Found a transition : " + transition);
                return transition;
            }
        }
            
        return null;
    }

    public void AddAnyTransition(IState to, IPredicate condition){
        //convert state to StateNode then add Any Transition to it
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    public void AddTransition(IState from, IState to, IPredicate condition){
        GetOrAddNode(from).AddTransition(to, condition);
        //Debug.Log("Added Transition from: " + from + " to " + to + " with description" + condition.Description);
    }
    StateNode GetOrAddNode(IState state){
        
        StateNode node = nodes.GetValueOrDefault(state.GetType());

        if (node == null){
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;

    }

    private class StateNode{
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state){
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition){
            Transitions.Add(new Transition(to, condition));
        }

    }
}
