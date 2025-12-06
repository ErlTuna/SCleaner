using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{
    public float InitialDelay = .1f;
    public float CooldownDuration = 1f;
    public bool IsOnCooldown = false;
    public bool IsExecuting = false;
    protected Coroutine cooldownCoroutine;
    public abstract IEnumerator Execute(BaseEnemyWeapon weapon);
    public abstract IEnumerator Cooldown(BaseEnemyWeapon weapon);

    void OnEnable()
    {
        IsOnCooldown = false;
        IsExecuting = false;
    }

    /// Checks in-case the weapon and/or its WeaponAnimator script gets destroyed somehow.
    protected bool IsWeaponAndAnimatorValid(BaseEnemyWeapon weapon)
    {
        return weapon != null && weapon.WeaponAnimator != null;
    }
}
