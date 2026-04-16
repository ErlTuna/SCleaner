using UnityEngine;

[CreateAssetMenu(fileName = "Max Health Increase", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Health/Increase Max HP")]
public class PickupBehaviour_IncreaseMaxHealth : PickupBehaviorSO
{
    public int Amount;
    public int HealOnApply;

    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<PlayerHealthManager>(out var healthManager))
        {
            healthManager.AddMaxHealthModifier(
                new IntModifier(Amount, ModifierOperation.Add),
                HealOnApply
            );
        }
    }
}
