using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public BoxCollider2D roomArea;
    public BoxCollider2D spawnTrigger;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> enemyList = new List<GameObject>();
    public LayerMask invalidSpawnLayers;
    public int roomID;
    public int enemyCount = 5;

    public void PopulateRoom(int increase = 0){
        if(enemyPrefabs.Count == 0){
            print("There are no enemies to populate the room with.");
            return;
        }
        enemyCount += increase;
        for(int i = 0; i < enemyCount; ++i){
                
                int random = Random.Range(0, enemyPrefabs.Count);
                GameObject instantiatedEnemy = Instantiate(enemyPrefabs[random], transform);
                Vector2 spawnPos = CalculateRandomSpawnLocation(roomArea);
                instantiatedEnemy.transform.position = spawnPos;
                instantiatedEnemy.transform.localRotation = Quaternion.identity;
                instantiatedEnemy.GetComponent<IEnemy>().SpawnArea = roomArea;
                enemyList.Add(instantiatedEnemy);
                instantiatedEnemy.SetActive(false);
            }
        print(enemyList.Count);
    }

    public void AwakenEnemiesWithinRoom(GameObject trigger){
        print(enemyList.Count);
        if (enemyList.Count == 0) return;

        for(int i = 0; i < enemyList.Count; ++i){
            if(enemyList[i] != null)
                enemyList[i].SetActive(true);
        }
        Destroy(trigger);
    }

    private Vector2 CalculateRandomSpawnLocation(BoxCollider2D spawnArea){
        Vector2 defaultSpawnPosition = spawnArea.transform.position;
        Vector2 spawnPos = Vector2.zero;
        bool isValidSpawnPos = false;
        float maxAttemptCount = 50;
        float currentAttemptCount = 0;

        while (!isValidSpawnPos && currentAttemptCount < maxAttemptCount){

            spawnPos = CalculateRandomPositionInCollider(spawnArea);
            Collider2D[] collidersInArea = Physics2D.OverlapCircleAll(spawnPos, 2f);
            bool isInvalidSpot = false;

            foreach (Collider2D coll in collidersInArea){
                if(((1 << coll.gameObject.layer) & invalidSpawnLayers) != 0){
                    isInvalidSpot = true;
                    break;
                }
            }

            if(!isInvalidSpot){
                isValidSpawnPos = true;
                return spawnPos;
            }

            currentAttemptCount++;
        }
        return defaultSpawnPosition;
    }

    ValueTuple<Vector2, Vector2> CalculateBoundMinMax(BoxCollider2D area, float offset = 5f){

        Vector2 minBounds = new Vector2(area.bounds.min.x, area.bounds.min.y);
        Vector2 maxBounds = new Vector2(area.bounds.max.x, area.bounds.max.y);
        return (minBounds, maxBounds);
    }

     Vector2 CalculateRandomPositionInCollider(BoxCollider2D boxCollider2D){

        (Vector2 minBounds, Vector2 maxBounds) = CalculateBoundMinMax(boxCollider2D);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        return new Vector2(randomX, randomY);
    }
}
