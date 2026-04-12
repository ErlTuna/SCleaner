using UnityEngine;

[CreateAssetMenu(fileName = "AddToInventory", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Items/AddToInventory")]
public class PickupBehaviour_AddItemToInventory : PickupBehaviorSO
{
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.PickupTransactionData.Payload is not PassiveItemPickupPayload) return;
        if (!ctx.TryGet<PlayerInventoryManager>(out var inventory)) return;

        PassiveItemPickupPayload itemPickupPayload = ctx.PickupTransactionData.Payload as PassiveItemPickupPayload;
        inventory.AddPassiveItem(itemPickupPayload);
    }
}
