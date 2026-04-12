using UnityEngine;

// Scriptable Object responsible for defining how a weapon attacks
// Alters a weapon's state and triggers animations if necessary

public abstract class FiringModeSO : ScriptableObject
{
    // Called when the player presses the trigger
    public abstract void OnTriggerPressed(IWeaponAttackInputHandler weapon);
    
    // Optional: called while trigger is held
    public virtual void OnTriggerHeld(IWeaponAttackInputHandler weapon) { }

    // Called when the player releases the trigger
    public abstract void OnTriggerReleased(IWeaponAttackInputHandler weapon);

    
}



