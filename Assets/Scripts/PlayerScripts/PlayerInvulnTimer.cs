using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvulnTimer : MonoBehaviour
{
    Coroutine _timerCoroutine;

    void OnEnable()
    {
        PlayerHealthManager.OnPlayerHitStateUpdate += StartInvulnTimer;
    }

    void OnDisable()
    {
        PlayerHealthManager.OnPlayerHitStateUpdate -= StartInvulnTimer;
    }
    [SerializeField] float _invulnTimer = 3f;

    public void StartInvulnTimer(UnitStateData stateData)
    {
        _timerCoroutine = StartCoroutine(TickInvulnTimerForHurt(stateData));
    }
    
    public IEnumerator TickInvulnTimerForHurt(UnitStateData stateData)
    {
        Debug.Log("Ticking timer for invuln");
        stateData.IsHitInvuln = true;
        yield return new WaitForSeconds(_invulnTimer);
        stateData.IsHitInvuln = false;
    }
    

}
