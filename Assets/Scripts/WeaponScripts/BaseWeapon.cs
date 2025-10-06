using System;
using System.Collections;
using UnityEngine;

// Composition root of weapons.

[Serializable]
[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(WeaponSoundManager))]
public abstract class BaseWeapon : MonoBehaviour
{
    public FiringModeSO PrimaryAttackStrategy;
    //public FiringModeSO SecondaryAttackStrategy;
    public WeaponAnimator WeaponAnimator;
    public AmmoManager AmmoManager;
    public WeaponConfigSO WeaponConfig;
    public WeaponRuntimeData WeaponRuntimeData;
    [SerializeField] protected WeaponSoundManager weaponSoundManager;
    [SerializeField] BoxCollider2D _boxCollider2D;
    [SerializeField] protected Transform[] FiringPoints;
    [SerializeField] protected Transform rayCastStartPoint;
    [SerializeField] protected Transform rayCastEndPoint;
    [SerializeField] protected Transform muzzleTipCheck;
    Coroutine _dryFireCoroutine;

    public virtual void InitializeWithConfig()
    {
        WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
    }
    public virtual void InitializeWithRuntimeData(WeaponRuntimeData runtimeData)
    {
        WeaponRuntimeData = runtimeData;
    }
    public abstract void HandlePrimaryAttackInput();
    public virtual void HandlePrimaryAttackInputCancel() { }
    public virtual void HandleSecondaryAttackInput() { }
    public abstract void HandleReloadInput();
    public abstract void HandleReloadStart();
    public abstract void SpawnBullet();
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
        GameObject pickup = TransformToPickup();
        //Toss(pickup, tossDirection);
        Destroy(gameObject);
    }

    public virtual void ResetWeapon()
    {
        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetAnimParams();
        GetComponent<SpriteRenderer>().sprite = WeaponConfig.Sprite;
        if (_dryFireCoroutine != null)
            StopCoroutine(_dryFireCoroutine);
        gameObject.SetActive(false);
    }

    public virtual void OnOutOfAmmo()
    {
        Debug.Log("Out of ammo...");
        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetAnimParams();
        //WeaponEvents.RaiseWeaponOutOfAmmo();

    }

    public virtual bool CanFire()
    {
        return WeaponRuntimeData.State == WeaponState.IDLE && AmmoManager.HasAmmo();
    }

    public virtual bool CanDryFire()
    {
        return !AmmoManager.HasAmmo() && !AmmoManager.CanReload();
    }

    public virtual void TryDryFire()
    {
        if (WeaponRuntimeData.State == WeaponState.DRY_FIRING) return;
        if (_dryFireCoroutine == null)
        {
            _dryFireCoroutine = StartCoroutine(DryFireCoroutine());
            return;
        }

        /*
        if (SETTINGS.AutoSwitchWhenNoAmmo)
            WeaponEvents.RaiseWeaponOutOfAmmo();
        */

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

    GameObject TransformToPickup()
    {
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.parent = null;

        GameObject pickupGO = WeaponPickupFactory.CreatePickupFromWeapon(this);
        return pickupGO;
    }

    protected virtual void Toss(GameObject pickup, Vector2 direction)
    {

        if (pickup.TryGetComponent(out Rigidbody2D rb) == false)
            rb = pickup.AddComponent<Rigidbody2D>();

        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
        //rb.AddTorque(10f, ForceMode2D.Impulse); // Adjust for style

    }

    protected virtual Quaternion CalculateBulletSpread()
    {

        if (Time.unscaledTime - WeaponRuntimeData.TimeSinceLastFired <= WeaponConfig.SpreadResetThreshold)
        {
            float angleDeviation = UnityEngine.Random.Range(-WeaponConfig.SpreadAngle, WeaponConfig.SpreadAngle);
            float baseAngle = FiringPoints[0].rotation.eulerAngles.z;
            float finalAngle = baseAngle + angleDeviation;
            return Quaternion.Euler(0, 0, finalAngle);
        }

        else
            return transform.rotation;
    }

}


