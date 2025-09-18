using System;

[Serializable]
public class PlayerHealthData : IUnitHealthData
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.HealthConfig);
    }

    public void ConfigureWith(UnitHealthConfigSO healthConfig)
    {
        MaxHealth = healthConfig.maxHealth;
        CurrentHealth = MaxHealth;
    }
}
