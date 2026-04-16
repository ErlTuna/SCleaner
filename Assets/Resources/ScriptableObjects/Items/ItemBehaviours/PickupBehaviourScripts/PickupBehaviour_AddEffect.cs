using UnityEngine;

[CreateAssetMenu(fileName = "Health Recovery", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Effects/Grant Effect")]
public class PickupBehaviour_AddEffect : PickupBehaviorSO
{
    public EffectSO effectSO; // ScriptableObject storing effect data

    public override void Apply(PickupExecutionContext ctx)
    {
        // Create a runtime instance from the SO
        IPersistentEffect effect = effectSO.CreateEffectInstance(); 
        EffectContext effectContext = new (ctx.Player.GetManagers());

        // Add it to the player's effect container
        ctx.Player.EffectContainer.Add(effect, effectContext);
    }
}
