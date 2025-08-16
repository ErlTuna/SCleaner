using UnityEngine;

public class EnemyWallDetection : MonoBehaviour
{

    public delegate void OnWallHit();
    public static OnWallHit onWallHit;

    public delegate void OnWallStuck();
    public static OnWallStuck onWallStuck;

    private float timer = 0f;
    private float wallStuckLimit = 1f;
    [SerializeField] LayerMask wallLayer;
    void OnTriggerEnter2D(Collider2D other){
        if((wallLayer.value & (1 << other.gameObject.layer)) > 0){
            //send a signal to the statemachine to find a new direction
            onWallHit?.Invoke();
        } 
    }

    void OnTriggerStay2D(Collider2D other){
        //if stuck in a wall, wait a second then forcefully detach, reset timer
        if((wallLayer.value & (1 << other.gameObject.layer)) > 0 ){
            timer += 0.1f;
        }
        if (wallStuckLimit <= timer) {
            onWallHit?.Invoke();
            timer = 0f;
        }
    }

}
