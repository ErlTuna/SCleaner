using System;
using System.Collections;
using UnityEngine;
using SerializeReferenceEditor;

// Composition root of weapons.

[Serializable]
[RequireComponent(typeof(WeaponSoundManager))]
public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected WeaponSoundManager weaponSoundManager;
    [SerializeField] protected Transform[] FiringPoints;
    [SerializeField] private WeaponAnimator _animatorComponent;
    IWeaponAnimator _weaponAnimator;
    public IWeaponAnimator WeaponAnimator => _weaponAnimator;
    public AmmoManager AmmoManager;
    public WeaponConfigSO WeaponConfig;
    public WeaponRuntimeData WeaponRuntimeData;

    void Awake()
    {
        if (_animatorComponent)
            _weaponAnimator = _animatorComponent;
    }

    public abstract void SpawnBullet();
    public virtual void InitializeWithConfig()
    {
        Debug.Log("Initialized weapon with config.");
        WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
    }
    public virtual void InitializeWithRuntimeData(WeaponRuntimeData runtimeData)
    {
        Debug.Log("Initialized weapon with config.");
        WeaponRuntimeData = runtimeData;
    }
    public virtual bool CanFire()
    {
        return WeaponRuntimeData.State == WeaponState.IDLE && AmmoManager.HasAmmo();
    }
    public abstract void OnAttackAnimEnd();
    public abstract void OnReloadAnimEnd();
}


