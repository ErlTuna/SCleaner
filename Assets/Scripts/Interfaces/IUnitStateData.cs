using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitStateData : IUnitRuntimeData, IConfigurable<UnitStateConfigSO>, IAutoConfigurable
{
    public bool IsAlive { get; set; }
    public bool IsInvuln { get; set; }
    public bool CanMove { get; set; }
    public bool IsShielded { get; set; }
}
