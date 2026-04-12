using UnityEngine;

[CreateAssetMenu(fileName = "CanBeHealed", menuName = "ScriptableObjects/Pickups/Pickup Conditions/Health/CanBeHealed")]
public class PickupCondition_CanBeHealed : PickupConditionSO
{
    public override bool Evaluate(PickupExecutionContext ctx)
    {
        return ctx.TryGet<IHealthPickupHandler>(out var health)
            && health.CanBeHealed();
    }



}
