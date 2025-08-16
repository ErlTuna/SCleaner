using UnityEngine;
using UnityEngine.AI;

/* public class OldRoamState : BaseState
{
    Vector2 direction;
    Vector2 lastKnownPlayerPosition = Vector2.zero;

    public OldRoamState(BugEnemy enemy, GameObject player, Rigidbody2D rb2D) : base(enemy, player, rb2D){
     }

    public override void OnEnter()
    {
        if(lastKnownPlayerPosition == Vector2.zero){
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        } else {
            direction = lastKnownPlayerPosition;
        }
        EnemyWallDetection.onWallHit += CalculateRandomDirection;
        EnemyWallDetection.onWallStuck += PushBack;
    }

    public override void OnExit()
    {
        rb2D.transform.eulerAngles = new Vector3(0, 0, 0);
        EnemyWallDetection.onWallHit -= CalculateRandomDirection;
        EnemyWallDetection.onWallStuck -= PushBack;
    }

    public override void StateUpdate()
    {
        //Debug.Log("roam state update");
        Rotate(rb2D.transform);
        
    }

    public override void StateFixedUpdate()
    {
        //Debug.Log("roam state fixedupdate");
        rb2D.velocity = 3f * direction;
    }

    private void CalculateRandomDirection(){
        CoordinateRegion currentDirectionRegion = CoordinateRegionManager.CheckRegion(direction.x, direction.y);
        float x, y;
        
        do {
        x = Random.Range(-1f, 1f);
        y = Random.Range(-1f, 1f);
        } while(currentDirectionRegion.x1 < x && x < currentDirectionRegion.x2 && currentDirectionRegion.y1 < y && y <currentDirectionRegion.y2);
        
        direction = new Vector2(x, y).normalized;
    }

    private void Rotate(Transform transform){
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private void PushBack(){
        direction *= -1f;
    }

} */


