using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] IntEventChannelSO _enemiesSpawnedEventChannel;
    public static EnemySpawnManager Instance{get; private set;}
    public GameObject[] roomsInScene;
    public Dictionary<int, GameObject> roomsDictionary = new();
    public Dictionary<int, RoomController> roomInfoDictionary = new();


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        
        else 
            Instance = this;
        
        
    }

    IEnumerator Start()
    {
        yield return null;
        if(roomsInScene.Length != 0)
            SetupRooms();
    }

    void SetupRooms()
    {
        print("Rooms in scene : " + roomsInScene.Length);
        for(int i = 0; i < roomsInScene.Length; ++i)
        {
            if(roomsInScene[i] != null)
            {
                RoomController roomScript = roomsInScene[i].GetComponent<RoomController>();
                roomsDictionary[i] = roomsInScene[i];
                roomInfoDictionary[i] = roomScript;
            }
        }

        PopulateRoomsWithEnemies();
    }

    void PopulateRoomsWithEnemies()
    {
        int totalEnemyCount = 0;

        for(int i = 0; i < roomsInScene.Length; ++i)
        {
            if (roomInfoDictionary[i].IsActive == false) continue;
            
            roomInfoDictionary[i].PopulateRoom();
            totalEnemyCount += roomInfoDictionary[i].EnemyCount;
        }

        _enemiesSpawnedEventChannel.RaiseEvent(totalEnemyCount);
    }

    
}
