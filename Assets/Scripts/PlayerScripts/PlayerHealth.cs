using System;
using System.Collections;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable, IHealth
{
    public HealthSO playerHealth;
    public UnitStateSO playerState;    
    public static event Action<int> OnPlayerHit;    
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


    public void Init(HealthSO healthData){
        //UnitInfo = info;
    }

    public void TakeDamage(IEnemy attacker){
        /* onPlayerDamaged?.Invoke(playerStats.isInvuln);
        playerStats.health -= 5;
        if (playerStats.health <= 0){
            playerStats.isAlive = false;
        } */
    }

    public void TakeDamage(int amount){

        if (playerState.isInvuln) return;
        if (!playerState.isAlive) return;

        playerHealth.health -= 10;
        OnPlayerHit?.Invoke(playerHealth.health);

        if (playerHealth.health <= 0)
        {
            playerState.isAlive = false;
            //PlayPlayerSounds.PlayAudio(PlayerSoundsSO.OnDefeatSFX);
            //spriteRenderer.sprite = UnitInfo.DefeatSprite;
            onPlayerDeath?.Invoke();
            return;
        }
        //PlayPlayerSounds.PlayAudio(UnitInfo.OnHitSFX);
        //StartCoroutine(onPlayerDamaged?.Invoke(UnitInfo));
        //StartCoroutine(onPlayerHitInvuln?.Invoke());
    }
}
