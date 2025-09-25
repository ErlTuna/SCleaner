
// This interface holds properties for Units in game that have health
using UnityEngine;

public interface IUnitHealthData : IUnitRuntimeData, IConfigurable<UnitHealthConfigSO>, IAutoConfigurable
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public AudioClip OnHitSFX { get; set; }
}
