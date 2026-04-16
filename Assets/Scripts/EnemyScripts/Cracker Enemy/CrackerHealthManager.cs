using System;
using UnityEngine;

public class CrackerHealthManager : MonoBehaviour, IDamageable, IDefeatable
{
    public event Action OnDefeat;
    public event Func<bool> OnBeforeDefeat;
    public event Action<DamageContext> OnDefeatWithContext;
    public IDetachable _hatScript;
    [SerializeField] UnitHealthData _healthData;
    UnitHealthConfigSO _healthConfig;
    UnitStateData _stateData;
    [SerializeField] DamageFlash _damageFlash;
    EnemyVisualConfigSO _visualConfig;

    public void InitializeManager(UnitHealthData healthData, UnitHealthConfigSO healthConfig, EnemyVisualConfigSO visualConfig, EnemyHat hatScript = null)
    {
        _healthData = healthData;
        _healthConfig = healthConfig;
        _hatScript = hatScript;
        _visualConfig = visualConfig;
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
            Debug.Log("Defeated...");
            if (_hatScript != null)
                _hatScript.Detach(context);

            OnDefeat?.Invoke();
            OnDefeatWithContext?.Invoke(context);
        }
    }
    
    void ApplyDamage(int amount)
    {
        _damageFlash.TriggerDamageFlash();

        if (_healthConfig.OnHitSFX)
            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, transform.position);

        _healthData.CurrentHealth -= amount;
        
    }

}
