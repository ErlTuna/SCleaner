using UnityEngine;

[CreateAssetMenu(fileName = "AddToInventory", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Weapons/AddToInventory")]
public class PickupBehaviour_AddWeaponToInventory : PickupBehaviorSO
{
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.PickupTransactionData.Payload is not WeaponPickupPayload) return;
        if (!ctx.TryGet<PlayerInventoryManager>(out var inventory)) return;

        WeaponPickupPayload weaponPickupPayload = ctx.PickupTransactionData.Payload as WeaponPickupPayload;
        //inventory.AddPickedUpWeapon(weaponPickupPayload);
        inventory.AddPickedUpWeapon(weaponPickupPayload);
    }
}
