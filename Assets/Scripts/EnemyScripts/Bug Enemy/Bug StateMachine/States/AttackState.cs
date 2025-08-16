using System.Collections;
using System.Threading;
using UnityEngine;

/* public class OldAttackState : BaseState
{
    private Vector2 playerDirection;
    private float maxAttackWaitTime = 1f;
    private float attackWaitTimer = 0f;
    private bool canAttack = true;
    private bool hasAttacked = false;

    public OldAttackState(BugEnemy enemy, GameObject player, Rigidbody2D rb2D) : base(enemy, player, rb2D){ }

    public override void OnEnter()
    {
        Debug.Log("Entering attack");
        rb2D.velocity = Vector2.zero;
        rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting attack");
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        attackWaitTimer = 0f;
    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {
        //Debug.Log(enemy.attackWaitTimer);
        Lunge();
        TickAttackWaitTimer();
        RotateTowardsPlayer();

    }

    void CalcPlayerDirection(){
        playerDirection = (player.transform.position - enemy.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        CalcPlayerDirection();
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        enemy.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void Lunge(){

        if(!canAttack) return;
        

        rb2D.velocity = Vector2.zero;
        rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        Debug.Log("Lunging!");
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2D.AddForce(playerDirection * 10f, ForceMode2D.Impulse);
        hasAttacked = true;
        canAttack = false;
        attackWaitTimer = 0f;
    }
    
    void TickAttackWaitTimer(){
        if(hasAttacked || attackWaitTimer < maxAttackWaitTime){
            attackWaitTimer += Time.deltaTime;
        }
        if (maxAttackWaitTime <= attackWaitTimer){
            canAttack = true;
        }
    }
} */
