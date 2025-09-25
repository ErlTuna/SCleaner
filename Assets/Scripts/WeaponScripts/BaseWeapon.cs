using System;
using UnityEngine;

// Composition root of weapons.

[Serializable]
public abstract class BaseWeapon : MonoBehaviour
{
    public FiringModeSO PrimaryAttackStrategy;
    //public FiringModeSO SecondaryAttackStrategy;
    public WeaponAnimator WeaponAnimator;
    public AmmoManager AmmoManager;
    public WeaponConfigSO WeaponConfig;
    public WeaponRuntimeData WeaponRuntimeData;
    [SerializeField] protected Transform[] FiringPoints;
    [SerializeField] protected Transform rayCastStartPoint;
    [SerializeField] protected Transform rayCastEndPoint;
    [SerializeField] protected Transform muzzleTipCheck;

    public virtual void InitializeWithData()
    {
        WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
    }
    public abstract void HandlePrimaryAttackInput();
    public virtual void HandlePrimaryAttackInputCancel() { }
    public virtual void HandleSecondaryAttackInput() { }
    public abstract void HandleReloadInput();
    public abstract void HandleReloadStart();
    public abstract void SpawnBullet();
    public virtual void SwitchFrom()
    {
        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetAnimParams();
        GetComponent<SpriteRenderer>().sprite = WeaponConfig.sprite;
        gameObject.SetActive(false);
    }

}
