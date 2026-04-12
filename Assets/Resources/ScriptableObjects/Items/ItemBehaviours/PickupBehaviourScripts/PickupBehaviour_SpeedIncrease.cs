using UnityEngine;

[CreateAssetMenu(fileName = "GrantSpeedIncrease", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Movement Speed/GrantSpeedIncrease")]
public class PickupBehaviour_SpeedIncrease : PickupBehaviorSO
{
    public float Amount;
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet(out PlayerMovementManager movementManager))
        {
            movementManager.AddMovementSpeedModifier(new FloatModifier(Amount, ModifierOperation.Add));
        }
    }
}
