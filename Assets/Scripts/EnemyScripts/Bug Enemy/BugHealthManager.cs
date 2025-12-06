using System;
using UnityEngine;
public class BugHealthManager : MonoBehaviour, IDamageable
{
    public Action OnHitReceived;
    public Action<DamageContext> OnDefeat;
    [SerializeField] UnitHealthData _healthData;
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitStateData _stateData;
    [SerializeField] DamageFlash _damageFlash;
    public void InitializeManager(UnitHealthData healthData, UnitHealthConfigSO healthConfig, EnemyVisualConfigSO visualConfig)
    {
        _healthData = healthData;
        _healthConfig = healthConfig;
        //_visualConfig = visualConfig;
    }
    
    public void InitializeStateData(EnemyStateData stateData)
    {
        _stateData = stateData;
    }

    public void TakeDamage(int amount)
    {
        if (_stateData == null)
        {
            Debug.Log("State Data is missing");
            return;
        }

        if (_stateData.IsAlive != true)
        {
            Debug.Log("Can't take damage while dead...");
            return;
        } 

        ApplyDamage(amount);
        
        if (_healthData.CurrentHealth <= 0)
        {
            _stateData.IsAlive = false;
            Debug.Log("I'm dead!!");
        }
    }


    public void TakeDamage(DamageContext context)
    {
        if (_stateData == null)
        {
            Debug.Log("State Data is missing");
            return;
        }

        if (_stateData.IsAlive != true)
        {
            Debug.Log("Can't take damage while dead...");
            return;
        } 

        ApplyDamage(context.Damage);

        if (_healthData.CurrentHealth <= 0)
        {
            OnDefeat?.Invoke(context);
        }
    }
    
    void ApplyDamage(int amount)
    {
        _damageFlash.TriggerDamageFlash();

        if (_healthConfig.OnHitSFX)
            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
         
        _healthData.CurrentHealth -= amount;
        //Debug.Log("Took : " + amount + " damage");
    }
}
