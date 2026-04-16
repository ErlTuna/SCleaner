using System;
using System.Linq;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable, IDefeatable, IHealable, IHealthPickupHandler, IManagerComponent
{
    [Header("Event Channels")]
    [SerializeField] PlayerHealthInitializedEventChannel _playerHealthInitializedEventChannel;
    [SerializeField] PlayerMaxHealthChangedEventChannel _playerMaxHealthChangedEventChannel;
    [SerializeField] PlayerHealthChangedEventChannel _playerHealthChangedEventChannel;
    [SerializeField] PlayerShieldHealthChangedEventChannel _playerShieldHealthChangedEventChannel;
    [SerializeField] PlayerHitUIEventChannelSO _playerHitEventChannel;

    [Header("Config and Data")]
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitHealthData _healthData;

    // EVENTS
    public static event Action<UnitStateData> OnPlayerHitStateUpdate;
    public static event Action OnPlayerHitSpriteUpdate;
    public event Func<bool> OnBeforeDefeat;
    public event Action OnDefeat;
    public event Action<DamageContext> OnDefeatWithContext;
    UnitStateData _stateData;

    public void InitializeManager(UnitHealthData healthData, UnitHealthConfigSO unitHealthConfig)
    {
        _healthData = healthData;
        _healthConfig = unitHealthConfig;
        _playerHealthInitializedEventChannel.RaiseEvent(_healthData);
    }
    
    public void InitializeStateData(UnitStateData playerStateData)
    {
        _stateData = playerStateData;
    }


    
    public void TakeDamage(int damage)
    {
        if (_stateData.IsAlive == false) return;
        if (_stateData.IsInvuln || _stateData.IsHitInvuln) return;
        
        int remainingDamage = damage;
        int previousHealth = _healthData.CurrentHealth;
        StatChangedArgs healthChangedArgs;

        if (_healthData.HasShield)
        {
            remainingDamage = ApplyDamageToShield(damage);
            //_healthData.CurrentHealth -= remainingDamage;
            _healthData.CurrentHealth = Mathf.Max(_healthData.CurrentHealth - remainingDamage, 0);
            healthChangedArgs = new()
            {
                Current = _healthData.CurrentHealth,
                Previous = previousHealth
            };
        }

        else
        {
            //_healthData.CurrentHealth -= remainingDamage;
            _healthData.CurrentHealth = Mathf.Max(_healthData.CurrentHealth - remainingDamage, 0);
            healthChangedArgs = new()
            {
                Current = _healthData.CurrentHealth,
                Previous = previousHealth
            };
        }
        
        

        if (_healthData.CurrentHealth <= 0)
        {
            if (OnBeforeDefeat != null)
            {
                foreach (Delegate subscriber in OnBeforeDefeat.GetInvocationList().Cast<Func<bool>>())
                {
                    Func<bool> interceptor = (Func<bool>)subscriber;
                    if (interceptor())
                    {
                        //Update State Data
                        OnPlayerHitStateUpdate?.Invoke(_stateData);

                        //For sprite blinking
                        OnPlayerHitSpriteUpdate?.Invoke();

                        if (_healthConfig.OnHitSFX != null) 
                            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);

                        return;
                    }  
                }
            }

            _stateData.IsAlive = false;
            if (healthChangedArgs.Delta < 0) _playerHealthChangedEventChannel.RaiseEvent(healthChangedArgs);

            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
            GameManager.Instance.SetGameState(GameState.PLAYER_DEFEAT);
            return;
        }

        //Update State Data
        OnPlayerHitStateUpdate?.Invoke(_stateData);

        //For sprite blinking
        OnPlayerHitSpriteUpdate?.Invoke();

        // UI Update and Audio Cue
        //if (_playerHitEventChannel != null) _playerHitEventChannel.RaiseEvent(_healthData.CurrentHealth, _healthData.MaxHealth.Value);
        if (healthChangedArgs.Delta < 0) _playerHealthChangedEventChannel.RaiseEvent(healthChangedArgs);
        if (_healthConfig.OnHitSFX != null) AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
    }
    

    public void TakeDamage(DamageContext context)
    {
        if (_stateData.IsAlive == false) return;
        if (_stateData.IsInvuln || _stateData.IsHitInvuln) return;
        
        int remainingDamage = context.Damage;
        int previousHealth = _healthData.CurrentHealth;
        StatChangedArgs healthChangedArgs;

        if (_healthData.HasShield)
        {
            remainingDamage = ApplyDamageToShield(context.Damage);
            _healthData.CurrentHealth -= remainingDamage;
            healthChangedArgs = new()
            {
                Current = _healthData.CurrentHealth,
                Previous = previousHealth
            };
        }

        else
        {
            _healthData.CurrentHealth -= remainingDamage;
            healthChangedArgs = new()
            {
                Current = _healthData.CurrentHealth,
                Previous = previousHealth
            };
        }

        
        
        if (_healthData.CurrentHealth <= 0)
        {
            if (OnBeforeDefeat != null)
            {
                foreach (Delegate subscriber in OnBeforeDefeat.GetInvocationList().Cast<Func<bool>>())
                {
                    Func<bool> interceptor = (Func<bool>)subscriber;
                    if (interceptor())
                    {
                        
                        //Update State Data
                        OnPlayerHitStateUpdate?.Invoke(_stateData);

                        //For sprite blinking
                        OnPlayerHitSpriteUpdate?.Invoke();

                        if (_healthConfig.OnHitSFX != null) 
                            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
                        return;
                    }  
                }
            }


            _stateData.IsAlive = false;
            if (healthChangedArgs.Delta < 0) _playerHealthChangedEventChannel.RaiseEvent(healthChangedArgs);

            AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
            GameManager.Instance.SetGameState(GameState.PLAYER_DEFEAT);
            return;
        }

        

        //Update State Data
        OnPlayerHitStateUpdate?.Invoke(_stateData);

        //For sprite blinking
        OnPlayerHitSpriteUpdate?.Invoke();

        // UI Update and Audio Cue
        //if (_playerHitEventChannel != null) _playerHitEventChannel.RaiseEvent(_healthData.CurrentHealth, _healthData.MaxHealth.Value);
        if (healthChangedArgs.Delta < 0) _playerHealthChangedEventChannel.RaiseEvent(healthChangedArgs);
        if (_healthConfig.OnHitSFX != null) AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);
    }

    public void Heal(int amount)
    {

        if (CanBeHealed() == false) return;
        
        
        int previousHealth = _healthData.CurrentHealth;
        _healthData.CurrentHealth = Mathf.Min(_healthData.CurrentHealth + amount, _healthData.MaxHealth.Value);

        StatChangedArgs healthChangedArgs = new()
        {
            Previous = previousHealth,
            Current = _healthData.CurrentHealth    
        };

        _playerHealthChangedEventChannel.RaiseEvent(healthChangedArgs);
        
    }
    public bool CanBeHealed()
    {
        if (_healthData.CurrentHealth >= _healthData.MaxHealth.Value)
        {
            Debug.Log("Health is greater than or equal to max health");
            return false;
        }

        return true;
    }

    public void AddMaxHealthModifier(IntModifier mod, int healOnApply = 0)
    {
        int previousMaxHealth = _healthData.MaxHealth.Value;
        _healthData.MaxHealth.AddModifier(mod);

        Debug.Log("Max health increased by : " + mod.Amount);

        StatChangedArgs maxHealthChangeArgs = new()
        {
            Previous = previousMaxHealth,
            Current = _healthData.MaxHealth.Value    
        };
    
        _playerMaxHealthChangedEventChannel.RaiseEvent(maxHealthChangeArgs);
        if (healOnApply != 0)
            Heal(healOnApply);
    }

    int ApplyDamageToShield(int amount)
    {
        int previousShieldHealth = _healthData.CurrentShieldHealth;
        int remainingDamage = amount - _healthData.CurrentShieldHealth;
        _healthData.CurrentShieldHealth = Mathf.Max(_healthData.CurrentShieldHealth - amount, 0);

        StatChangedArgs shieldHealthChangeArgs = new()
        {
            Previous = previousShieldHealth,
            Current = _healthData.CurrentShieldHealth
        };

        _playerShieldHealthChangedEventChannel.RaiseEvent(shieldHealthChangeArgs);
        
        if (remainingDamage <= 0)
            return 0;
        else 
            return remainingDamage;
    }

    public void AddShieldHP(int amount)
    {
        int previousShieldHealth = _healthData.CurrentShieldHealth;
        _healthData.CurrentShieldHealth += amount;

        StatChangedArgs shieldChangeArgs = new()
        {
            Previous = previousShieldHealth,
            Current = _healthData.CurrentShieldHealth    
        };

        _playerShieldHealthChangedEventChannel.RaiseEvent(shieldChangeArgs);
    }
    

}

