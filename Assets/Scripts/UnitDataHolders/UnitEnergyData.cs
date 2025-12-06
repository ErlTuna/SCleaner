
using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class UnitEnergyData
{
    public float CurrentEnergy;
    public float MaxEnergy;
    public float RechargeInterval;
    public float RechargeRate;

    public UnitEnergyData(UnitEnergyConfigSO EnergyConfig)
    {
        CurrentEnergy = EnergyConfig.maxEnergy;
        MaxEnergy = EnergyConfig.maxEnergy;
        RechargeInterval = EnergyConfig.rechargeInterval;
        RechargeRate = EnergyConfig.rechargeRate;;
    }
}
