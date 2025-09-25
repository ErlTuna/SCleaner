using System;
using UnityEngine;
public class BugHealth_v2 : MonoBehaviour, IHealthComponent
{
    Unit _owner;
    public Action OnHitReceived;
    public Action<DamageContext> OnDefeat;
    public UnitHealthData HealthData;
    UnitStateData stateData;
    [SerializeField] DamageFlash _damageFlash;
    Vector2 lastHitDirection;

    public void Initialize(Unit owner)
    {
        _owner = owner;
        if (_owner.RuntimeDataHolder.TryGetRuntimeData(out UnitHealthData health))
            HealthData = health;

        if (_owner.RuntimeDataHolder.TryGetRuntimeData(out EnemyStateData data))
            stateData = data;

    }

    public void TakeDamage(int amount)
    {
        if (stateData == null)
        {
            Debug.Log("State Data is missing");
            return;
        }

        if (stateData.IsAlive != true)
        {
            Debug.Log("Can't take damage while dead...");
            return;
        } 

        ApplyDamage(amount);
        
        if (HealthData.CurrentHealth <= 0)
        {
            stateData.IsAlive = false;
            Debug.Log("I'm dead!!");
        }
    }


    public void TakeDamage(DamageContext context)
    {
        if (stateData == null)
        {
            Debug.Log("State Data is missing");
            return;
        }

        if (stateData.IsAlive != true)
        {
            Debug.Log("Can't take damage while dead...");
            return;
        } 

        ApplyDamage(context.Damage);

        if (HealthData.CurrentHealth <= 0)
        {
            OnDefeat?.Invoke(context);
        }
    }
    
    void ApplyDamage(int amount)
    {
        _damageFlash.TriggerDamageFlash();

        if (HealthData.OnHitSFX)
            AudioSource.PlayClipAtPoint(HealthData.OnHitSFX, gameObject.transform.position);
         
        HealthData.CurrentHealth -= amount;
        //Debug.Log("Took : " + amount + " damage");
    }
}
