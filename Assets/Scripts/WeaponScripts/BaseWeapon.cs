using System;
using UnityEngine;

// Composition root of weapons.

[Serializable]
[RequireComponent(typeof(WeaponSoundManager))]
public abstract class BaseWeapon<TConfig> : MonoBehaviour
where TConfig : WeaponConfigSO
{
    
    # region Serialize Fields
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected WeaponSoundManager weaponSoundManager;
    [SerializeField] protected Transform[] FiringPoints;
    [SerializeField] protected WeaponAnimatorBase weaponAnimatorComponent;
    [SerializeField] protected AmmoManager ammoManager;
    IWeaponAnimator weaponAnimator;

    // Config and Runtime Data
    [SerializeField] protected TConfig weaponConfig;
    [SerializeField] protected WeaponRuntime weaponRuntimeData;

    # endregion
    
    # region Getters
    public AmmoManager AmmoManager => ammoManager;
    public IWeaponAnimator WeaponAnimator => weaponAnimator;
    public WeaponRuntime WeaponRuntimeData
    {
        get
        {
            if (weaponRuntimeData == null)
            {
                Debug.LogWarning($"{name} was not initialized. Creating fallback runtime data.");
                //weaponRuntimeData = InitializeWithConfig();
            }
            return weaponRuntimeData;
        }
    }
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public TConfig WeaponConfig => weaponConfig;

    # endregion

    # region Abstract Methods
    protected abstract void SpawnBullet();
    protected abstract void OnPrimaryAttackAnimEnd();
    protected abstract void OnReloadAnimEnd();

    # endregion

    # region Virtual Methods

    protected virtual void Awake()
    {
        if (weaponAnimatorComponent)
            weaponAnimator = weaponAnimatorComponent;
    }

    protected virtual ReloadContext CreateReloadContext()
    {
        return new ReloadContext
        (
            runtimeAmmoData: WeaponRuntimeData.AmmoData,
            ammoConfig: weaponConfig.AmmoConfig
        );
    }
    
    public virtual void LoopFire()
    {
        //Debug.Log("Looping fire");
        weaponAnimatorComponent.LoopPrimaryAttackAnim();
    }

    public virtual void RequestFire()
    {
        //Debug.Log("Fire requested at : " + Time.time);
        SetState(WeaponState.PRIMARY_ATTACK);
        weaponAnimatorComponent.StartPrimaryAttackAnim();
    }
    public virtual bool CanFire() => WeaponRuntimeData.State == WeaponState.IDLE && AmmoManager.HasAmmo();
    protected virtual bool CanContinuePrimaryAttack() => AmmoManager.HasAmmo() == true;
    protected virtual void OnInitialized() => AmmoManager.Initialize(weaponRuntimeData.AmmoData);
    

    # endregion

    # region Initialization Methods

    // Called when accessing WeaponRuntimeData if it is not initialized yet.
    WeaponRuntime InitializeWithConfig()
    {
        WeaponAmmoData ammoData = new(WeaponConfig.AmmoConfig);
        weaponRuntimeData = new(WeaponConfig, ammoData);
        Debug.Log("Initialized weapon with weapons' own config.");

        return weaponRuntimeData;
    }

    public void InitializeWithRuntimeData(WeaponRuntime runtimeData)
    {
        if (weaponRuntimeData != null)
        {
            Debug.LogWarning($"{name} initialized twice.");
            return;
        }


        Debug.Log("Initialized weapon with provided runtime data.");
        weaponRuntimeData = runtimeData;
        OnInitialized();
    }

    # endregion

    public void SetState(WeaponState state)
    {
        WeaponRuntimeData.State = state;
    }
}


