using System;
using UnityEngine;
[Serializable]
public class UnitHealthData
{
    public int CurrentHealth;
    public int MaxHealth;
    public int DamageThreshold;
    public UnitHealthData(UnitHealthConfigSO healthConfig)
    {
        MaxHealth = healthConfig.MaxHealth;
        CurrentHealth = MaxHealth;
        DamageThreshold = healthConfig.DamageThreshold;
    }
}
