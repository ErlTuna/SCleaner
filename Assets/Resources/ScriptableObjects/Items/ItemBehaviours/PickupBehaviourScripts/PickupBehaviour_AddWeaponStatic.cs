using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Behaviors/AddWeaponStatic")]
public class PickupBehavior_AddWeaponStatic : PickupBehaviorSO
{
    public PlayerWeaponConfigSO WeaponConfig;

    public override void Apply(PickupExecutionContext ctx)
    {
        if (!ctx.TryGet<PlayerInventoryManager>(out var inventory)) return;
        //inventory.AddWeaponFromConfig(WeaponConfig);
    }
}

