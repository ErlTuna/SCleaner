using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitRuntimeDataHolder RuntimeDataHolder;
    public UnitConfigsWrapperSO UnitConfigWrapper;

    public virtual void InitializeDataWithConfigWrapper(UnitConfigsWrapperSO unitConfig)
    {
        RuntimeDataHolder?.InitializeWithWrapper(this, unitConfig);
    }

    public T GetRuntimeData<T>() where T : class, IUnitRuntimeData
    {
        return RuntimeDataHolder?.GetRuntimeData<T>();
    }

    public bool TryGetRuntimeData<T>(out T component) where T : class, IUnitRuntimeData
    {
        component = RuntimeDataHolder?.GetRuntimeData<T>();
        return component != null;
    }
    
}