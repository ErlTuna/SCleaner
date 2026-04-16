using UnityEngine;

/*
[CreateAssetMenu(menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Weapon")]
public class WeaponPickupBehaviorSO : PickupBehaviorSO
{
    public override void Apply(PickupContext ctx, ItemPickup pickup)
    {
        if (pickup is not WeaponPickup weaponPickup)
        {
            Debug.LogError("WeaponPickupBehaviorSO applied to a non-weapon pickup!");
            return;
        }

       if(ctx.Player.GetComponent<PlayerMain>().TryGetManager(out PlayerInventoryManager inventory))
        {
            inventory.TryAddWeapon(weaponPickup.Weapon);
        }
    }
}
*/
