using System;
using SerializeReferenceEditor;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable, IDefeatable, IHealable
{
    [SerializeField] PlayerHitUIEventChannelSO _playerHitEventChannel;
    [SerializeField] UnitHealthConfigSO _healthConfig;
    [SerializeField] UnitHealthData _healthData;
    public static event Action<UnitStateData> OnPlayerHitStateUpdate;
    public static event Action OnPlayerHitSpriteUpdate;
    public static event Action<int, int> OnPlayerHitUIUpdate;
    public event Action OnDefeat;
    public event Action<DamageContext> OnDefeatContext;


    UnitStateData _stateData;

    public void InitializeManager(UnitHealthData healthData, UnitHealthConfigSO unitHealthConfig)
    {
        _healthData = healthData;
        _healthConfig = unitHealthConfig;
    }
    
    public void InitializeStateData(UnitStateData playerStateData)
    {
        _stateData = playerStateData;
    }


    public void TakeDamage(int amount)
    {

        if (_stateData.IsAlive == false) return;
        
        if (_stateData.IsInvuln)
        {
            Debug.Log("Invuln due to an external effect");
            return;
        }
        if (_stateData.IsHitInvuln)
        {
            Debug.Log("Invuln due to taking damage recently");
            return;
        }        

        _healthData.CurrentHealth -= amount;

        //Update State Data
        OnPlayerHitStateUpdate?.Invoke(_stateData);

        //For sprite blinking
        OnPlayerHitSpriteUpdate?.Invoke();

        // UI Update and Audio Cue
        if (_playerHitEventChannel != null) _playerHitEventChannel.RaiseEvent(_healthData.CurrentHealth, _healthData.MaxHealth);
        if (_healthConfig.OnHitSFX != null) AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);

        if (_healthData.CurrentHealth <= 0)
        {
            //_stateData.IsAlive = false;

            //Update game state
            GameManager.Instance.SetGameState(GameState.PLAYER_DEFEAT);
            return;
        }
    }

    public void TakeDamage(DamageContext context)
    {
        if (_stateData.IsAlive == false) return;
        
        if (_stateData.IsInvuln)
        {
            Debug.Log("Invuln due to an external effect");
            return;
        }
        
        if (_stateData.IsHitInvuln)
        {
            Debug.Log("Invuln due to taking damage recently");
            return;
        }        

        _healthData.CurrentHealth -= context.Damage;

        //Update State Data
        OnPlayerHitStateUpdate?.Invoke(_stateData);

        //For sprite blinking
        OnPlayerHitSpriteUpdate?.Invoke();

        // UI Update and Audio Cue
        if (_playerHitEventChannel != null) _playerHitEventChannel.RaiseEvent(_healthData.CurrentHealth, _healthData.MaxHealth);
        if (_healthConfig.OnHitSFX != null) AudioSource.PlayClipAtPoint(_healthConfig.OnHitSFX, gameObject.transform.position);

        if (_healthData.CurrentHealth <= 0)
        {
            //_stateData.IsAlive = false;

            //Update game state
            GameManager.Instance.SetGameState(GameState.PLAYER_DEFEAT);
            return;
        }
    }

    public void Heal(int amount)
    {
        Debug.Log("Attempting heal on player...");

        if (_healthData.CurrentHealth >= _healthData.MaxHealth)
        {
            Debug.Log("Health is greater than or equal to max health");
            return;
        }

        Debug.Log("Healing" + gameObject.name + "by : " + amount);
        _healthData.CurrentHealth += amount;


        if (_healthData.CurrentHealth > _healthData.MaxHealth)
            _healthData.CurrentHealth = _healthData.MaxHealth;

        Debug.Log("The health value is now : " + _healthData.CurrentHealth);
    }

}

