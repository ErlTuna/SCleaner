using UnityEngine;

// Scriptable Object responsible for defining how a weapon attacks
// Alters a weapon's state and triggers animations if necessary

public abstract class FiringModeSO : ScriptableObject
{
    public string Description;
    public virtual void HandleDirectFire(BaseWeapon weapon)
    {
        HandleAttackStart(weapon);
    }
    public virtual void HandlePreAttack(BaseWeapon weapon) { }
    public virtual void HandleAttackCanceled(BaseWeapon weapon) { }
    public virtual void HandlePreAttackEnd(BaseWeapon weapon) { }
    public abstract void HandleAttackStart(BaseWeapon weapon);
    public abstract void HandleAttackEnd(BaseWeapon weapon);

}

