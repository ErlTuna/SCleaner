using UnityEngine;

[CreateAssetMenu(fileName = "ChargeFiringModeSO", menuName = "ScriptableObjects/Weapon/FiringModes/ChargeFiringModeSO")]
public class ChargeFiringModeSO : FiringModeSO
{
    public override void OnTriggerPressed(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IChargeFiringWeapon chargeFiringWeapon && chargeFiringWeapon.CanCharge())
        {
            chargeFiringWeapon.BeginCharge();
        }
    }

    public override void OnTriggerHeld(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IChargeFiringWeapon chargeFiringWeapon)
        {
            // Animation stuff in the future, perhaps
        }
    }

    public override void OnTriggerReleased(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IChargeFiringWeapon chargeFiringWeapon)
        {
            chargeFiringWeapon.CancelCharge();
        }
    }
}
