using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : BaseWeapon
{
    Coroutine _dryFireCoroutine;
    public FiringModeSO PrimaryAttackStrategy;
    [SerializeField] protected Transform rayCastStartPoint;
    [SerializeField] protected Transform rayCastEndPoint;
    [SerializeField] protected Transform muzzleTipCheck;
    public abstract void HandlePrimaryAttackInput();
    public virtual void HandlePrimaryAttackInputCancel() { }
    public virtual void HandleSecondaryAttackInput() { }
    public abstract void HandleReloadInput();
    public abstract void HandleReloadStart();
    public virtual void ResetWeapon()
    {
        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetAnimParams();
        _spriteRenderer.sprite = WeaponConfig.Sprite;
        if (_dryFireCoroutine != null)
            StopCoroutine(_dryFireCoroutine);
        gameObject.SetActive(false);
    }

    public virtual void TryDryFire()
    {
        if (WeaponRuntimeData.State == WeaponState.DRY_FIRING) return;
        if (_dryFireCoroutine == null)
        {
            _dryFireCoroutine = StartCoroutine(DryFireCoroutine());
            return;
        }

        IEnumerator DryFireCoroutine()
        {
            Debug.Log("Attempting to dry fire...");
            WeaponRuntimeData.State = WeaponState.DRY_FIRING;
            weaponSoundManager.TryPlaySound(WeaponConfig.DryFiringSound);
            yield return new WaitForSeconds(.3f);
            WeaponRuntimeData.State = WeaponState.IDLE;
        }

        _dryFireCoroutine = null;
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

    public virtual void SwitchFrom()
    {
        ResetWeapon();
    }
    public virtual bool CanBeDropped()
    {
        return WeaponRuntimeData.State == WeaponState.IDLE;
    }
    public virtual void Drop()
    {
        ResetWeapon();
        //Vector2 tossDirection = gameObject.transform.parent.right;
        //GameObject pickup = TransformToPickup();
        TransformToPickup();
        //Toss(pickup, tossDirection);
        Destroy(gameObject);
    }

    GameObject TransformToPickup()
    {
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.parent = null;

        GameObject pickupGO = WeaponPickupFactory.CreatePickupFromWeapon(this);
        return pickupGO;
    }

    public virtual bool CanDryFire()
    {
        return !AmmoManager.HasAmmo() && !AmmoManager.CanReload();
    }
    public override void OnAttackAnimEnd()
    {
        PrimaryAttackStrategy.HandleAttackEnd(this);
    }

    public override void OnReloadAnimEnd()
    {
        AmmoManager.UseReloadStrategy();
        //WeaponEvents.RaiseWeaponReload(WeaponRuntimeData);
        AmmoManager.ShouldContinueReloading();
    }

}
