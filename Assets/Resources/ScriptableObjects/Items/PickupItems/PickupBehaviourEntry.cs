using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

// Wrapper class to display order and subconditions for this behaviour
public class PickupBehaviorEntry
{
    public PickupBehaviorSO Behavior;
    public int Order;
    public bool IsExhaustible = false;
    public PickupConditionSO[] BehaviourSpecificConditions;

    public void TryApply(PickupExecutionContext ctx)
    {
        if (ctx == null)
        {
            Debug.Log("Context is null! Aborting!");
            return;
        }

        
        if (BehaviourSpecificConditions == null)
        {
            Behavior.Apply(ctx);
            return; // skip this behavior if any condition fails
        }


        foreach (var cond in BehaviourSpecificConditions)
        {
            if (cond.Evaluate(ctx) == false)
                return;
        }

        Behavior.Apply(ctx);
    }
}

