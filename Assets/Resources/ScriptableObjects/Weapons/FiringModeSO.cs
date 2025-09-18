using UnityEngine;

// Scriptable Object responsible for defining how a weapon attacks
// Alters a weapon's state and triggers animations if necessary

public abstract class FiringModeSO : ScriptableObject
{
    public string Description;
    public virtual void HandlePreAttack(BaseWeapon_v2 weapon) { }
    public virtual void HandleAttackCanceled(BaseWeapon_v2 weapon) { }
    public virtual void HandlePreAttackEnd(BaseWeapon_v2 weapon) { }
    public abstract void HandleAttackStart(BaseWeapon_v2 weapon);
    public abstract void HandleAttackEnd(BaseWeapon_v2 weapon);

}

