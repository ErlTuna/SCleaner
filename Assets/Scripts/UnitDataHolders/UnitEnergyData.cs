
using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class UnitEnergyData : IUnitEnergyData
{
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; set; }
    public float RechargeInterval { get; set; }
    public float RechargeRate { get; set; }
    public AudioClip FullEnergySFX { get; set; }

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
        FullEnergySFX = EnergyConfig.fullEnergySFX;

        Debug.Log("Initialized energy data, current energy " + CurrentEnergy);
    }
}
