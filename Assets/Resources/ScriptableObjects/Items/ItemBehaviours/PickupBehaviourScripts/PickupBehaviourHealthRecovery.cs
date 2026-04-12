using UnityEngine;

[CreateAssetMenu(fileName = "Health Recovery", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Health/Recover Health")]
public class PickupBehaviourHealthRecovery : PickupBehaviorSO
{
    public int amount;
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<IHealthPickupHandler>(out var health))
        {
            health.Heal(amount);
        }
    }
}
