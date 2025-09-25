using System;
using UnityEngine;
[Serializable]
public class UnitHealthData : IUnitHealthData
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public AudioClip OnHitSFX { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.HealthConfig);
    }

    public void ConfigureWith(UnitHealthConfigSO healthConfig)
    {
        MaxHealth = healthConfig.MaxHealth;
        CurrentHealth = MaxHealth;
        OnHitSFX = healthConfig.OnHitSFX;
    }
}
