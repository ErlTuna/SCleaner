using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable, IHealable
{
    Unit _owner;
    public IUnitHealthData HealthData;
    public static event Action<UnitStateData> OnPlayerHit;
    public static event Action OnPlayerHitSpriteUpdate;
    public static event Action<int> OnPlayerHitUIUpdate;

    void OnEnable()
    {
        //PlayerHitbox.onPlayerHitTakeDamage += TakeDamage;
    }

    void OnDisable()
    {
        //PlayerHitbox.onPlayerHitTakeDamage -= TakeDamage;
    }


    public void InitializeWithData(Unit owner, UnitHealthData healthData)
    {
        _owner = owner;
        HealthData = healthData;
        //Debug.Log("Got " + healthData);
        //Debug.Log("HealthData is now " + healthData.CurrentHealth);
    }




    public void TakeDamage(IEnemy attacker)
    {
        //no op
    }

    public void TakeDamage(int amount)
    {
        if (_owner.RuntimeDataHolder.TryGetRuntimeData(out UnitStateData stateData) != true) return;
        if (stateData.IsInvuln)
        {
            Debug.Log("Invuln due to an external effect");
            return;
        }
        if (stateData.IsHitInvuln)
        {
            Debug.Log("Invuln due to taking damage recently");
            return;
        }
        if (!stateData.IsAlive)
        {
            Debug.Log("Can't take damage while dead...");
            return;
        } 

        HealthData.CurrentHealth -= amount;
        Debug.Log("My health now... " + HealthData.CurrentHealth);

        //Update UI
        //OnPlayerHit?.Invoke(healthData.CurrentHealth);

        if (HealthData.CurrentHealth <= 0)
        {
            stateData.IsAlive = false;

            //Update game state
            //onPlayerDeath?.Invoke();
            return;
        }
        else
        {
            OnPlayerHit?.Invoke(stateData);
            OnPlayerHitSpriteUpdate?.Invoke();
            OnPlayerHitUIUpdate?.Invoke(HealthData.CurrentHealth);
            if (HealthData.OnHitSFX != null)
                AudioSource.PlayClipAtPoint(HealthData.OnHitSFX, gameObject.transform.position);
        }
    }

    /*
    public void TakeDamage(int amount)
    {

        if (_owner.RuntimeDataHolder.TryGetRuntimeData<UnitStateData>(out var stateData) != true) return;
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


        if (_owner.TryGetRuntimeComponent<IUnitState>(out var state))
        {
            if (state.IsInvuln || !state.IsAlive) return;
        }

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);

        if (CurrentHealth <= 0)
            Debug.Log("health has hit zero");
        

    }
    */

    public void TakeDamage(DamageContext context)
    {
        //Debug.Log("Taking damage... : " + context.Damage);
    }

    public void Heal(int amount)
    {
        Debug.Log("Attempting heal on player...");

        if (HealthData.CurrentHealth >= HealthData.MaxHealth)
        {
            Debug.Log("Health is greater than or equal to max health");
            return;
        }

        Debug.Log("Healing" + gameObject.name + "by : " + amount);
        HealthData.CurrentHealth += amount;


        if (HealthData.CurrentHealth > HealthData.MaxHealth)
            HealthData.CurrentHealth = HealthData.MaxHealth;

        Debug.Log("The health value is now : " + HealthData.CurrentHealth);
    }
}



public interface IHealthComponent : IDamageable
{

}