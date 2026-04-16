using System;
using UnityEngine;


public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] RoomController _roomScript;
    bool _isTriggered = false;

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!_isTriggered && collision.gameObject.CompareTag("Player"))
        {
            _isTriggered = true;
            _roomScript.AwakenEnemiesWithinRoom(gameObject);
        }
    }

}

