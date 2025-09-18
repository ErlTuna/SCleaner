using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeFiringMode", menuName = "Weapons/FiringModes/Charge Firing Mode")]
public class ChargeFiringModeSO : FiringModeSO
{
    public override void HandlePreAttack(BaseWeapon_v2 weapon)
    {
        
        if (weapon.WeaponRuntimeData.State != WeaponState.PRE_PRIMARY_ATTACK)
        {
            Debug.Log($"Before : {weapon.WeaponRuntimeData.State}");
            weapon.WeaponRuntimeData.State = WeaponState.PRE_PRIMARY_ATTACK;
            Debug.Log($"After : {weapon.WeaponRuntimeData.State}");
            weapon.WeaponAnimator.SetBool("isTriggerHeld", true);
        }

    }

    public override void HandleAttackCanceled(BaseWeapon_v2 weapon)
    {
        Debug.Log("odd");
        weapon.WeaponRuntimeData.State = WeaponState.IDLE;
        weapon.WeaponAnimator.SetBool("isTriggerHeld", false);
    }

    public override void HandlePreAttackEnd(BaseWeapon_v2 weapon)
    {
        Debug.Log("Weapon charged");
        HandleAttackStart(weapon);
    }

    public override void HandleAttackStart(BaseWeapon_v2 weapon)
    {
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
