using System;
using System.Collections;

public interface IWeaponAnimator
{
    void StartPrimaryAttackAnim();
    void LoopPrimaryAttackAnim();
    void StartReloadAnim();
    void ResetAnimParams();
    void SetBool(string name, bool value);
    void SetTrigger(string name);
    void SetFloat(string name, float value);
    void SetInt(string name, int value);
    IEnumerator WaitForAnimation(string stateName);
}

public interface IChargeBasedWeaponAnimator : IWeaponAnimator
{
    event Action OnChargeCompleted;
    event Action OnChargeCanceled;
    void StartChargeAnimation();
    void CancelChargeAnimation();
    
}

public interface IShellBasedWeaponAnimator : IWeaponAnimator
{
    void LoopReload();
    event Action OnShellInserted;
}

