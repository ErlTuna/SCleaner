using UnityEngine;

[CreateAssetMenu(fileName = "Health Recovery", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Health/Recover Shield HP")]
public class PickupBehaviour_RecoverShieldHP : PickupBehaviorSO
{
    public int amount;
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<IHealthPickupHandler>(out var health))
        {
            health.AddShieldHP(amount);
        }
    }
}
