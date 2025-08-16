using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Test : MonoBehaviour
{
    StateMachine sm;
    GameObject player;
    [SerializeField] Rigidbody2D rb2D;

    public float x = 0;

    

    void Awake(){
        /* player = gameObject;
        sm = new StateMachine();
        RoamState_v2 rs_v2 = new RoamState_v2(rb2D, player);
        StopState stopState = new StopState(rb2D, player);


        At(rs_v2, stopState, new FuncPredicate( () => x > 5));
        At(stopState, rs_v2, new FuncPredicate( () => x < 5));


        sm.SetState(rs_v2); */
    }

    void At(IState from, IState to, IPredicate condition) => sm.AddTransition(from, to, condition);
    void Start(){
        
    }

   void Update(){
    if (x < 5) { x += 0.0001f; }

        //sm.Update();
   
    }


}
 

