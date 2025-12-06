using System;

[Serializable]
public class EquipmentData
{
    public string EquipmentName;
    public EquipmentConfigSO Config {get; private set;}
    public EquipmentState State;
    public EquipmentAbilityConfig Ability;
    public float CurrentCharge;
    public int MaxCharge;
    public float Lifetime;


    public EquipmentData(EquipmentConfigSO equipmentConfigSO)
    {
        State = EquipmentState.INACTIVE;
        Config = equipmentConfigSO;
        MaxCharge = Config.StartingCarge;
        CurrentCharge = Config.MaxCharge;
        Lifetime = Config.MaxLifetime;
        Ability = equipmentConfigSO.Ability;
    }

}
