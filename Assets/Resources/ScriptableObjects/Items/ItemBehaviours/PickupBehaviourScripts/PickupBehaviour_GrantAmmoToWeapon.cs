using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Recovery", menuName = "ScriptableObjects/Pickups/Pickup Behaviours/Ammo/Grant Ammo")]
public class PickupBehaviour_GrantAmmoToWeapon : PickupBehaviorSO
{
    public AmmoPickupType AmmoPickupType;
    public override void Apply(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<PlayerWeaponManager>(out var weaponManager) == false) return;
        
        weaponManager.CurrentWeaponScript.AmmoManager.AddReserveAmmo(AmmoPickupType);
    }
}
