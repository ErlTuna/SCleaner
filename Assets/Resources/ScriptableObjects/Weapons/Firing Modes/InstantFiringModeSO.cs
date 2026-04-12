using UnityEngine;

[CreateAssetMenu(fileName = "InstantFiringMode", menuName = "ScriptableObjects/Weapon/FiringModes/InstantFiringMode")]
public class InstantFiringModeSO : FiringModeSO
{
    public override void OnTriggerPressed(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IInstantFiringWeapon instantFiringWeapon)
            instantFiringWeapon.RequestFire();
    }

    public override void OnTriggerHeld(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IInstantFiringWeapon instantFiringWeapon)
            instantFiringWeapon.LoopFire();
        
            
    }

    public override void OnTriggerReleased(IWeaponAttackInputHandler weapon)
    {
        if (weapon is IInstantFiringWeapon instantFiringWeapon)
            instantFiringWeapon.OnAttackInputReleased();
    }
}
