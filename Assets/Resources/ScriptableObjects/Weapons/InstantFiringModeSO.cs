using UnityEngine;

[CreateAssetMenu(fileName = "InstantFiringMode", menuName = "Weapons/FiringModes/InstantFiringMode")]
public class InstantFiringModeSO : FiringModeSO
{

    public override void HandleAttackStart(BaseWeapon_v2 weapon)
    {
        if (weapon.AmmoManager.HasAmmo() == false) return;

        weapon.WeaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        weapon.WeaponAnimator.StartPrimaryAttackAnim();
    }

    public override void HandleAttackEnd(BaseWeapon_v2 weapon)
    {
        weapon.WeaponAnimator.ResetAnimParams();

        if (PlayerInputManager.instance.PrimaryAttackInput == true && weapon.AmmoManager.HasAmmo())
        {
            weapon.WeaponRuntimeData.State = WeaponState.IDLE;
            HandlePreAttack(weapon);
        }

        else
            weapon.WeaponRuntimeData.State = WeaponState.IDLE;
    }
}
