using System;
using UnityEngine;


public class RoomEnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] RoomController _roomScript;
    bool _isTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_roomScript.IsRoomTriggered == false && collision.gameObject.CompareTag("Player"))
        {
            _isTriggered = true;
            _roomScript.AwakenEnemiesWithinRoom(gameObject);
        }
    }

}

