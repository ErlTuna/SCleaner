using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
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
        
        if(roomsInScene.Length != 0)
            SetupRooms();
    }

    void SetupRooms()
    {
        print("Rooms in scene : " + roomsInScene.Length);
        for(int i = 0; i < roomsInScene.Length; ++i)
        {
            if(roomsInScene[i] != null){
                RoomController roomScript = roomsInScene[i].GetComponent<RoomController>();
                //roomScript.RoomID = i;
                roomsDictionary[i] = roomsInScene[i];
                roomInfoDictionary[i] = roomScript;
            }
        }

        PopulateRoomsWithEnemies();
    }

    void PopulateRoomsWithEnemies()
    {
        Debug.Log("Attempting to populate room.");

        for(int i = 0; i < roomsInScene.Length; ++i)
        {
            roomInfoDictionary[i].PopulateRoom();
        }
    }

    
}
