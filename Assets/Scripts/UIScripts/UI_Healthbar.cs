using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    [SerializeField] PlayerHitUIEventChannelSO _playerHitUIEventChannel;
    [SerializeField] Image imageComponent;
    [SerializeField] Sprite[] _healthbarSprites;
    [SerializeField] int _healthStateCount = 10;

    void OnEnable()
    {
        //PlayerHealthManager.OnPlayerHitUIUpdate += UpdateHealthbar; 
        _playerHitUIEventChannel.OnEventRaised += UpdateHealthbar;
    }

    void OnDisable()
    {
        //PlayerHealthManager.OnPlayerHitUIUpdate -= UpdateHealthbar;
        _playerHitUIEventChannel.OnEventRaised -= UpdateHealthbar;
    }

    void UpdateHealthbar(int currentHealth, int currentMaxHealth)
    {
        //int healthState = (int)CalculateHealthState(currentHealth, currentMaxHealth);
        int healthState = currentHealth;
        if (_healthbarSprites[healthState] != null)
            imageComponent.sprite = _healthbarSprites[healthState];
    }

    /*
    HealthState CalculateHealthState(int value, int currentMaxHealth)
    {
        HealthState healthState = (HealthState)(value / currentMaxHealth);
        return healthState;
    }*/
}


