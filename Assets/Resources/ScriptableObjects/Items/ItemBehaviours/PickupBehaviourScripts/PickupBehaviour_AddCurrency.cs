
using UnityEngine;

[CreateAssetMenu(fileName = "AddToInventory", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Weapons/AddToInventory")]
public class PickupBehaviour_AddCurrency : PickupBehaviorSO
{
    public int Amount;
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<PlayerInventoryManager>(out var inventory) == false) return;

        inventory.AddCurrency(Amount);
    }
}
