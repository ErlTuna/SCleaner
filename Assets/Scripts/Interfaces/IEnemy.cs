using System;
using System.Collections;
using UnityEngine;

public interface IEnemy
{
    public EnemySO EnemyInfo{get;set;}
    public BoxCollider2D SpawnArea{get;set;}
    void SetProvoked();
    void PlayerIsDetected();
    void PlayerOutOfRange();
    void PlayerWithinAttackRange();
    void PlayerOutOfAttackRange();
    Coroutine TriggerCoroutine(IEnumerator coroutine);
    void CancelCoroutine(Coroutine coroutine);
}
