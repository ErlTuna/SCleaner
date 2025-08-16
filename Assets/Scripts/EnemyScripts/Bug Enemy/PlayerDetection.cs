using UnityEngine;

public class PlayerDetection : MonoBehaviour
{    

    IEnemy script;
    
    void Start(){
        script = GetComponentInParent<IEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            script.PlayerIsDetected();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            script.PlayerOutOfRange();
        }
    }
}
