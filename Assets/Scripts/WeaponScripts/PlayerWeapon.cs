using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerWeapon : BaseWeapon<PlayerWeaponConfigSO>
{
    // Make this a getter later?..
    Transform _ownerTransform;
    public FiringModeSO PrimaryAttackStrategy;
    [SerializeField] protected Transform rayCastStartPoint;
    [SerializeField] protected Transform rayCastEndPoint;
    [SerializeField] protected Transform muzzleTipCheck;

    # region Abstract Methods

    public abstract void HandlePrimaryAttackInput();
    public abstract void HandleReloadInput();
    public abstract void HandlePrimaryAttackInputRelease();

    # endregion

    # region Virtual Methods
    //public virtual void HandleSecondaryAttackInput() { }
    
    protected virtual void CheckAmmoStatus()
    {
        //weapon runs out of ammo but has reserve and isn't actively reloading, try starting a reload
        if (AmmoManager.HasAmmo() == false && AmmoManager.CanReload()  && WeaponRuntimeData.State != WeaponState.RELOADING)
        {
            TriggerOutOfAmmoReload();
        }
    }

    protected virtual void TriggerOutOfAmmoReload()
    {
        ReloadContext reloadContext = CreateReloadContext();
        AmmoManager.SetReloadContext(reloadContext);
        SetState(WeaponState.RELOADING);
        WeaponAnimator.StartReloadAnim();
    }

    public virtual void ResetWeapon()
    {

        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetAnimParams();
        spriteRenderer.sprite = WeaponConfig.Sprite;
        gameObject.SetActive(false);

    }

    protected virtual Quaternion CalculateBulletSpread()
    {

        if (Time.unscaledTime - WeaponRuntimeData.TimeSinceLastFired <= WeaponConfig.SpreadResetThreshold)
        {
            float angleDeviation = Random.Range(-WeaponConfig.SpreadAngle, WeaponConfig.SpreadAngle);
            float baseAngle = FiringPoints[0].rotation.eulerAngles.z;
            float finalAngle = baseAngle + angleDeviation;
            return Quaternion.Euler(0, 0, finalAngle);
        }

        else
            return transform.rotation;

    }

    # endregion

    # region Auxiliary Methods

    void TransformToPickup()
    {
        //gameObject.transform.localPosition = Vector3.zero;
        //gameObject.transform.parent = null;
        //WeaponPickupFactory.Create_v2(weaponRuntimeData, transform.position);
        WeaponPickupFactory.Create_v2(weaponRuntimeData, _ownerTransform.position);
        Destroy(gameObject);
    }

    public void Drop()
    {
        ResetWeapon();
        TransformToPickup();
    }

    public bool CanBeDropped()
    {
        return WeaponRuntimeData.State == WeaponState.IDLE && WeaponRuntimeData.CanBeDropped;
    }

    public void SwitchFrom()
    {
        ResetWeapon();
    }

    public void AttachOwnerTransform(Transform ownerTransform)
    {
        _ownerTransform = ownerTransform;
    }

    # endregion




}
