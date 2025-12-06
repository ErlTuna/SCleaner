using SerializeReferenceEditor;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IFactionMember
{
    [SerializeField] private Faction faction;
    public Faction Faction => faction;
    public abstract UnitStateData GetStateData();
}

    /*
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
    */