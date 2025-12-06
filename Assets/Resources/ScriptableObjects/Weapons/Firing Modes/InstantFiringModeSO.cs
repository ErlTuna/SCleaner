using UnityEngine;

[CreateAssetMenu(fileName = "InstantFiringMode", menuName = "ScriptableObjects/Weapon/FiringModes/InstantFiringMode")]
public class InstantFiringModeSO : FiringModeSO
{

    public override void HandleAttackStart(BaseWeapon weapon)
    {
        if (weapon.AmmoManager.HasAmmo() == false) return;

        weapon.WeaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        weapon.WeaponAnimator.StartPrimaryAttackAnim();
    }

    public override void HandleAttackEnd(BaseWeapon weapon)
    {
        weapon.WeaponAnimator.ResetAnimParams();

        if (PlayerInputManager.Instance.PrimaryAttackInput == true && weapon.AmmoManager.HasAmmo())
        {
            weapon.WeaponRuntimeData.State = WeaponState.IDLE;
            HandlePreAttack(weapon);
        }

        else
            weapon.WeaponRuntimeData.State = WeaponState.IDLE;
    }
}
