using UnityEngine;

public abstract class PickupConditionSO : ScriptableObject
{
    public string Name;
    public string Description;
    public abstract bool Evaluate(PickupExecutionContext ctx);
}
