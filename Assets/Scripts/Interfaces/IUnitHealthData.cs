
// This interface holds properties for Units in game that have health
public interface IUnitHealthData : IUnitRuntimeData, IConfigurable<UnitHealthConfigSO>, IAutoConfigurable
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
}
