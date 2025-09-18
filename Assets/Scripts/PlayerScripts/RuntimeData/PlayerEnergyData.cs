
using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class PlayerEnergyData : IUnitEnergyData, IAutoConfigurable
{
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; set; }
    public float RechargeInterval { get; set; }
    public float RechargeRate { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.EnergyConfig);
    }

    public void ConfigureWith(UnitEnergyConfigSO EnergyConfig)
    {
        CurrentEnergy = EnergyConfig.maxEnergy;
        MaxEnergy = EnergyConfig.maxEnergy;
        RechargeInterval = EnergyConfig.rechargeInterval;
        RechargeRate = EnergyConfig.rechargeRate;

        Debug.Log("Initialized energy data, current energy " + CurrentEnergy);
    }
}
