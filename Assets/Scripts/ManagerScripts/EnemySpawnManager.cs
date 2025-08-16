using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance{get; private set;}
    public GameObject[] roomsInScene;
    public Dictionary<int, GameObject> roomsDictionary = new Dictionary<int, GameObject>();
    public Dictionary<int, Room> roomInfoDictionary = new Dictionary<int, Room>();    
    public int iteration = 0;
    public int increase = 5;
    void Awake(){
        if (instance != null && instance != this){
            Destroy(this);
        } else {
            instance = this;
        }
        if(roomsInScene.Length != 0)
            SetupRooms();
    }

    void SetupRooms(){
        print("Rooms in scene : " + roomsInScene.Length);
        for(int i = 0; i < roomsInScene.Length; ++i){
            if(roomsInScene[i] != null){
                Room roomScript = roomsInScene[i].GetComponent<Room>();
                roomScript.roomID = i;
                roomsDictionary[i] = roomsInScene[i];
                roomInfoDictionary[i] = roomScript;
            }
        }
        PopulateRoomsWithEnemies();
    }
    void PopulateRoomsWithEnemies(){
        for(int i = 0; i < roomsInScene.Length; ++i){
            roomInfoDictionary[i].PopulateRoom();
        }
    }

    
}
