using System;


[Serializable]
public class PlayerEnergyData
{
    public float CurrentEnergy;
    public float MaxEnergy;
    public float RechargeInterval;
    public float RechargeRate;

    public PlayerEnergyData(PlayerEnergyConfigSO EnergyConfig)
    {
        CurrentEnergy = EnergyConfig.maxEnergy;
        MaxEnergy = EnergyConfig.maxEnergy;
        RechargeInterval = EnergyConfig.rechargeInterval;
        RechargeRate = EnergyConfig.rechargeRate;;
    }
}
