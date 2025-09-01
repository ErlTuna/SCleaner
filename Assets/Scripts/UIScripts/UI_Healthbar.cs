using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    [SerializeField] Image imageComponent;
    [SerializeField] Sprite[] _healthbarSprites;

    void OnEnable()
    {
        PlayerHealth.OnPlayerHit += UpdateHealthbar; 
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= UpdateHealthbar; 
    }

    void UpdateHealthbar(int currentHealth)
    {
        int healthState = (int)CalculateHealthState(currentHealth);
        if (_healthbarSprites[healthState] != null)
            imageComponent.sprite = _healthbarSprites[healthState];
    }

    HealthState CalculateHealthState(int value)
    {
        HealthState healthState = (HealthState)(value / 10);
        return healthState;
    }
}

public enum HealthState
{
    HP_0,
    HP_10,
    HP_20,
    HP_30,
    HP_40,
    HP_50,
    HP_60,
    HP_70,
    HP_80,
    HP_90,
    HP_100
}
