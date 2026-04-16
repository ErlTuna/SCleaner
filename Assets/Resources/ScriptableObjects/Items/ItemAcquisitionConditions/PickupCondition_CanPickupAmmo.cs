using UnityEngine;

[CreateAssetMenu(fileName = "CanPickupAmmo", menuName = "ScriptableObjects/Pickups/Pickup Conditions/Ammo/CanPickupAmmo")]
public class PickupCondition_CanPickupAmmo : PickupConditionSO
{
    public override bool Evaluate(PickupExecutionContext ctx)
    {
        if (ctx.TryGet<PlayerWeaponManager>(out var weaponManager) == false) return false;
        if (weaponManager.CurrentWeaponScript == null) return false;
        if (weaponManager.CurrentWeaponScript.AmmoManager.CanPickupReserveAmmo() == false) 
        {
            Debug.Log("WEAPON HAS INFINITE RESERVE AMMO!");
            return false;
        }
        
        return true;
    }
}
