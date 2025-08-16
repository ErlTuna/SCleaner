using System;
using UnityEngine;


public class EnemySpawnTrigger : MonoBehaviour
{
    public Room roomScript;
    bool isTriggered = false;

    void OnTriggerExit2D(Collider2D collision){
        print(roomScript);
        // collision.gameObject.CompareTag("Player") && 
        //if (collision.transform.position.y - transform.position.y < 0.001f && !isTriggered){
            if(!isTriggered && collision.gameObject.CompareTag("Player")){
            isTriggered = true;
            roomScript.AwakenEnemiesWithinRoom(gameObject);
            }
            //EnemySpawnManager.instance.StartCoroutine(EnemySpawnManager.instance.SpawnEnemies());
            //Destroy(gameObject);
        }

}

