using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealth, ISetup
{
    public UnitInfoSO UnitInfo{get;set;}    
    public delegate IEnumerator OnPlayerDamaged(UnitInfoSO info);
    public static OnPlayerDamaged onPlayerDamaged;
    
    public delegate IEnumerator OnPlayerHitInvulnEventHandler();
    public static OnPlayerHitInvulnEventHandler onPlayerHitInvuln;
    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    public SpriteRenderer spriteRenderer;

    void OnEnable(){
        PlayerHitbox.onPlayerHitTakeDamage += TakeDamage;   
    }

    void OnDisable(){
        PlayerHitbox.onPlayerHitTakeDamage -= TakeDamage;
    }


    public void Init(UnitInfoSO info){
        UnitInfo = info;
    }

    public void TakeDamage(IEnemy attacker){
        /* onPlayerDamaged?.Invoke(playerStats.isInvuln);
        playerStats.health -= 5;
        if (playerStats.health <= 0){
            playerStats.isAlive = false;
        } */
    }

    public void TakeDamage(int amount){

        if (UnitInfo.isInvuln) return;
        if (!UnitInfo.isAlive) return;

        UnitInfo.health -= amount;
        if (UnitInfo.health <= 0){
            UnitInfo.isAlive = false;
            PlayPlayerSounds.PlayAudio(UnitInfo.OnDefeatSFX);
            spriteRenderer.sprite = UnitInfo.DefeatSprite;
            onPlayerDeath?.Invoke();
            return;
        }
        PlayPlayerSounds.PlayAudio(UnitInfo.OnHitSFX);
        StartCoroutine(onPlayerDamaged?.Invoke(UnitInfo));
        StartCoroutine(onPlayerHitInvuln?.Invoke());
    }
}
