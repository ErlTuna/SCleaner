using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvulnTimer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    float _invulnTimer = 2f;
    bool hurtInvuln = false;

    void OnEnable(){
        PlayerHealth.onPlayerDamaged += TickInvulnTimerForHurt;
        PlayerDash.onDashTriggered += TickInvulnTimerForDash;
    }   

    void OnDisable(){
        PlayerHealth.onPlayerDamaged -= TickInvulnTimerForHurt;
        PlayerDash.onDashTriggered -= TickInvulnTimerForDash;
    }

    IEnumerator TickInvulnTimerForHurt(UnitInfoSO playerInfo){
        playerInfo.isInvuln = true;
        _spriteRenderer.sprite = playerInfo.HurtSprite;
        yield return new WaitForSeconds(_invulnTimer);
        _spriteRenderer.sprite = playerInfo.DefaultSprite;
        playerInfo.isInvuln = false;
    }

    IEnumerator TickInvulnTimerForDash(UnitInfoSO playerInfo, float dashDuration){
        if (hurtInvuln) yield break;
        playerInfo.isInvuln = true;
        yield return new WaitForSeconds(dashDuration);
        playerInfo.isInvuln = false;
    }
    

}
