using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable, IHealthComponent
{
    Unit _owner;
    public PlayerHealthData HealthData;
    public static event Action<int> OnPlayerHit;
    public delegate IEnumerator OnPlayerHitInvulnEventHandler();
    public static OnPlayerHitInvulnEventHandler onPlayerHitInvuln;
    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    public float CurrentHealth { get; set; }

    public float MaxHealth { get; set; }



    void OnEnable()
    {
        PlayerHitbox.onPlayerHitTakeDamage += TakeDamage;
    }

    void OnDisable()
    {
        PlayerHitbox.onPlayerHitTakeDamage -= TakeDamage;
    }


    public void InitializeWithData(Unit owner, PlayerHealthData healthData)
    {
        _owner = owner;
        HealthData = healthData;
    }




    public void TakeDamage(IEnemy attacker)
    {

    }

    public void TakeDamage(int amount)
    {

        if (_owner.RuntimeDataHolder.TryGetRuntimeData<PlayerStateData>(out var stateData) != true) return;
        if (stateData.IsInvuln) return;
        if (!stateData.IsAlive) return;

        HealthData.CurrentHealth -= 10;
        //OnPlayerHit?.Invoke(healthData.CurrentHealth);

        if (HealthData.CurrentHealth <= 0)
        {
            stateData.IsAlive = false;
            //PlayPlayerSounds.PlayAudio(PlayerSoundsSO.OnDefeatSFX);
            //spriteRenderer.sprite = UnitInfo.DefeatSprite;
            onPlayerDeath?.Invoke();
            return;
        }
        //PlayPlayerSounds.PlayAudio(UnitInfo.OnHitSFX);
        //StartCoroutine(onPlayerDamaged?.Invoke(UnitInfo));
        //StartCoroutine(onPlayerHitInvuln?.Invoke());
        

        /*if (_owner.TryGetRuntimeComponent<IUnitState>(out var state))
        {
            if (state.IsInvuln || !state.IsAlive) return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);

        if (CurrentHealth <= 0)
            Debug.Log("health has hit zero");
        */

    }

    public void TakeDamage(float amount)
    {
        throw new NotImplementedException();
    }

    public void Heal(float amount)
    {
        throw new NotImplementedException();
    }

}
public interface IHealthComponent
{
    //public IUnitHealthDataComponent HealthDataComponent{ get; set; }
    public void TakeDamage(float amount)
    {

    }

    public void Heal(float amount)
    {

    }
}