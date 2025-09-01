using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponData data;
    public Transform firingPoint;
    public Animator _animator;
    public RuntimeAnimatorController _runtimeAnimatorController;
    public event Action<BaseWeapon> OnWeaponFireEvent;
    public event Action OnWeaponFiredVisuals;
    public event Action<BaseWeapon> OnWeaponReloadEvent;

    public abstract void Reload();
    public abstract void HandlePrimaryAttackInput();
    public abstract void HandlePrimaryAttackInputCancel();
    public abstract void HandleFiringAnimationEnd();
    public abstract void HandleReloadStart();
    public abstract void HandleReloadEnd();
    public abstract void PrimaryAttack();
    public bool HasAmmo() { return data.currentAmmo > 0; }
    public abstract void ResetWeaponState();

}
