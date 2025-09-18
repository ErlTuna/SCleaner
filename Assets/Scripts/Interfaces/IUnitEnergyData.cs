using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitEnergyData : IUnitRuntimeData, IConfigurable<UnitEnergyConfigSO>
{
    public float CurrentEnergy { get; set; }
    public float MaxEnergy { get; set; }
    public float RechargeInterval { get; set; }
    public float RechargeRate { get; set; }
}
