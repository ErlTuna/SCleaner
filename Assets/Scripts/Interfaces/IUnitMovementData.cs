using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitMovementData : IUnitRuntimeData, IConfigurable<UnitMovementConfigSO>, IAutoConfigurable
{
    public float CurrentMovementSpeed { get; set; }
}
