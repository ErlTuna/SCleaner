using UnityEngine;

public abstract class PickupBehaviorSO : ScriptableObject
{
    [SerializeField] int _order;
    public int Order => _order;

    // Optional conditions per behavior
    public PickupConditionSO[] BehaviourSpecificConditions; 

    public abstract void Apply(PickupExecutionContext ctx);
    public virtual void Remove(PickupExecutionContext ctx)
    {
        // Only relevant for WhileOwned behaviors
    }
}