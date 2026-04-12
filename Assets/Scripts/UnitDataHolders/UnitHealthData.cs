using System;


[Serializable]
public class UnitHealthData
{
    public int CurrentHealth;
    public IntStat MaxHealth;
    public int CurrentShieldHealth;
    public bool HasShield => CurrentShieldHealth > 0;
    public UnitHealthData(UnitHealthConfigSO healthConfig)
    {
        MaxHealth = new IntStat(healthConfig.MaxHealth);
        CurrentHealth = MaxHealth.Value;
        CurrentShieldHealth = healthConfig.StartingShieldHealth;
    }
}
