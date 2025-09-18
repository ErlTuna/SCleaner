using System;
using System.Collections.Generic;
using System.Linq;


// Base class for RuntimeData used by units, it encapsulates all the data a Unit may hold
// Health data, energy data etc.

[Serializable]
public abstract class UnitRuntimeDataHolder
{
    public Unit Owner;
    protected readonly Dictionary<Type, IUnitRuntimeData> _datas = new();

    // Initialization method 
    // ----------------------------------------------------------------------------------------------------------------
    // Given a UnitConfigWrapper, if datas within are Automatically Configurable (implements IAutoConfigurable) 
    // it is configured automatically using a bridging IAutoConfigurable interface
    // IAutoConfigurable has a method which takes an IConfigWrapper reference
    // The interface then defers the work to the IConfigurable<TConfig> interface.

    // Using the Config(TConfig) method where TConfig is a generic,
    // the expected type is determined by the extending interface
    // Ex :
    // public interface IUnitInventoryData : IUnitRuntimeData, IConfigurable<UnitInventoryConfigSO>, IAutoConfigurable
    // InventoryData -> Expects InventoryConfig, simply put.
    // ----------------------------------------------------------------------------------------------------------------
    public void InitializeWithWrapper(Unit owner, UnitConfigsWrapperSO unitConfigsWrapper)
    {
        Owner = owner;

        foreach (var data in _datas.Values.Distinct())
        {
            if (data is IAutoConfigurable auto)
                auto.AutoConfigureWithWrapper(unitConfigsWrapper);
        }
    }

    public T GetRuntimeData<T>() where T : class, IUnitRuntimeData => _datas.TryGetValue(typeof(T), out var data) ? data as T : null;

    public void AddRuntimeData<T>(T data) where T : IUnitRuntimeData
    {
        Type concreteType = data.GetType();

        // Register under the actual runtime type (concrete class)
        if (!_datas.ContainsKey(concreteType))
            _datas[concreteType] = data;

        // Register under the generic type T (could be an interface)
        if (!_datas.ContainsKey(typeof(T)))
            _datas[typeof(T)] = data;


        // auxiliary, register under all interfaces
        foreach (var iface in concreteType.GetInterfaces())
        {
            if (typeof(IUnitRuntimeData).IsAssignableFrom(iface))
            {
                if (!_datas.ContainsKey(iface))
                    _datas[iface] = data;
            }
        }
    }

    public bool TryGetRuntimeData<T>(out T data) where T : class, IUnitRuntimeData
    {
        data = GetRuntimeData<T>();
        return data != null;
    }

    public IEnumerable<IUnitRuntimeData> GetAllRuntimeData() => _datas.Values;

}


/*public abstract class UnitRuntimeData_v2
{
    protected readonly Dictionary<Type, IUnitRuntimeData> _datas = new();
    public T GetRuntimeData<T>() where T : class, IUnitRuntimeData => _datas.TryGetValue(typeof(T), out var data) ? data as T : null;
    
}*/
