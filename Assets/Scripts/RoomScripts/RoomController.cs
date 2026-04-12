using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{   
    [SerializeField] Transform _lootSpawnLocation;
    [SerializeField] BoxCollider2D SpawnArea;
    [SerializeField] BoxCollider2D RoomArea;
    [SerializeField] BoxCollider2D SpawnTrigger;
    [SerializeField] List<GameObject> EnemyPrefabs;
    [SerializeField] List<GameObject> EnemyList = new();
    [SerializeField] LayerMask InvalidSpawnLayers;
    //[SerializeField] int RoomID;
    [SerializeField] int EnemyCount = 0;
    [SerializeField] int _aliveEnemyCount = 0;

    List <Vector2> _spawnLocations;


    public void PopulateRoom(int increase = 0)
    {
        if(EnemyPrefabs.Count == 0){
            print("There are no enemies to populate the room with.");
            return;
        }

        _spawnLocations = new(EnemyCount);

        for(int i = 0; i < EnemyCount; ++i)
        {
                
            int random = Random.Range(0, EnemyPrefabs.Count);
            GameObject instantiatedEnemy = Instantiate(EnemyPrefabs[random], transform);
            //Vector2 spawnPos = CalculateRandomSpawnLocation(RoomArea);

            Vector2 spawnPos = CalculateRandomSpawnLocation(SpawnArea);
            
            instantiatedEnemy.transform.position = spawnPos;
            instantiatedEnemy.transform.localRotation = Quaternion.identity;

            IEnemy enemy = instantiatedEnemy.GetComponent<IEnemy>();
            enemy.AssignSpawnArea(RoomArea);
            enemy.AssignOnDefeatCallback(OnEnemyDefeat);
            
            _spawnLocations.Add(spawnPos);
            EnemyList.Add(instantiatedEnemy);
            instantiatedEnemy.SetActive(false);
        }

        _aliveEnemyCount = EnemyCount;
        

        print(EnemyList.Count);
    }

    public void AwakenEnemiesWithinRoom(GameObject trigger)
    {
        print(EnemyList.Count);
        if (EnemyList.Count == 0) return;

        for(int i = 0; i < EnemyList.Count; ++i)
        {
            if(EnemyList[i] != null)
            {
                GameObject enemy = EnemyList[i];
                Vector2 location = _spawnLocations[i];

                SpawnEffectManager.Instance.PlaySpawnVFX(location, () =>
                {
                    // Callback fires when VFX finishes, enemy is awakened.
                    enemy.SetActive(true);
                });
            }
                
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
                if(((1 << coll.gameObject.layer) & InvalidSpawnLayers) != 0){
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

    ValueTuple<Vector2, Vector2> CalculateBoundMinMax(BoxCollider2D area, float offset = 5f)
    {

        Vector2 minBounds = new(area.bounds.min.x, area.bounds.min.y);
        Vector2 maxBounds = new(area.bounds.max.x, area.bounds.max.y);
        return (minBounds, maxBounds);
    }

     Vector2 CalculateRandomPositionInCollider(BoxCollider2D boxCollider2D)
    {

        (Vector2 minBounds, Vector2 maxBounds) = CalculateBoundMinMax(boxCollider2D);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        return new Vector2(randomX, randomY);
    }

    void OnEnemyDefeat()
    {
        Debug.Log("Invoked...");
        --_aliveEnemyCount;
        if (_aliveEnemyCount == 0)
            OnRoomCleared();
        
    }

    void OnRoomCleared()
    {
        LootManager.Instance.TrySpawnLootChest(_lootSpawnLocation.position);
    }
}
