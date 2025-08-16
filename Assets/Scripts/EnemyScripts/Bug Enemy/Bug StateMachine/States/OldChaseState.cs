/* using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class OldChaseState : BaseState
{

    private Vector2 playerDirection;
    //private Vector2 lastKnownPlayerPosition;


    public OldChaseState(BugEnemy enemy, GameObject player, Rigidbody2D rb2D) : base(enemy, player, rb2D){

    }

    public override void OnEnter()
    {
        //Debug.Log("Entering player chase");
    }

    public override void OnExit()
    {
        //Debug.Log("Exiting player chase");
        //lastKnownPlayerPosition = playerDirection;
        //Debug.Log(lastKnownPlayerPosition);
    }

    public override void StateUpdate()
    {
        RotateTowardsPlayer();
        MoveTowardsPlayer();
    }

    public override void StateFixedUpdate(){

    }

    private void RotateTowardsPlayer(){
        
        CalcPlayerDirection();
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void MoveTowardsPlayer(){
        rb2D.velocity = playerDirection * 3f;
    }

    private void CalcPlayerDirection(){
        playerDirection = (player.transform.position - enemy.transform.position).normalized;
    }

    
} */
