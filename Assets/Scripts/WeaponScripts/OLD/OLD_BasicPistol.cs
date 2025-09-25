using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_BasicPistol : OLD_BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    void Start()
    {
        weaponRuntimeData = new WeaponRuntimeData(weaponConfig);
    }

    void Update(){
        if(!HasAmmo() && weaponRuntimeData.ReserveAmmo != 0 && weaponRuntimeData.State != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && weaponRuntimeData.ReserveAmmo == 0 && weaponRuntimeData.State != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
        }
    }

    public void SetFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
    }


    public override void PrimaryAttack()
    {
        if (!HasAmmo() || weaponRuntimeData.State == WeaponState.RELOADING) return;


        weaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        Bullet instantiatedBullet = Instantiate(weaponConfig.BulletData.Prefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        //instantiatedBullet.SetupBulletParameters(weaponConfig.BulletData.ProjectileSpeed, weaponConfig.BulletData.Size, weaponConfig.Damage, weaponConfig.BulletData.LifeTime);


        --weaponRuntimeData.CurrentAmmo;
        WeaponEvents.RaiseWeaponFired(weaponRuntimeData);
        
        weaponRuntimeData.State = WeaponState.IDLE;
    }

    public override void Reload()
    {
        int ammoBeforeReload = weaponRuntimeData.CurrentAmmo;
        //weaponRuntimeData.CurrentAmmo = 0;
        weaponRuntimeData.State = WeaponState.RELOADING;

        if (weaponConfig.RoundCapacity - ammoBeforeReload <= weaponRuntimeData.ReserveAmmo)
        {
            weaponRuntimeData.ReserveAmmo -= weaponConfig.RoundCapacity - ammoBeforeReload;
            weaponRuntimeData.CurrentAmmo = weaponConfig.RoundCapacity;
        }
        else
        {
            weaponRuntimeData.CurrentAmmo += weaponRuntimeData.ReserveAmmo;
            weaponRuntimeData.ReserveAmmo = 0;
        }

        weaponRuntimeData.State = WeaponState.IDLE;
        WeaponEvents.RaiseWeaponReload(weaponRuntimeData);
    }

    void OnDrawGizmosSelected(){
        //Gizmos.DrawWireSphere(muzzleTipCheck.position, 0.5f);
        //Debug.DrawLine(rayCastStartPoint.position, rayCastEndPoint.position, Color.green);
    }

    public override void HandlePrimaryAttackInput()
    {

        if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint, rayCastEndPoint, environmentLayers, enemyLayer)) return;
        if (!HasAmmo() || weaponRuntimeData.State == WeaponState.RELOADING || weaponRuntimeData.State == WeaponState.PRIMARY_ATTACK) return;

        _animator.SetBool("isFiring", true);
        
    }

    public override void HandlePrimaryAttackInputCancel(){
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
    }

    public override void HandleReloadStart(){

        if(weaponRuntimeData.ReserveAmmo == 0 || weaponRuntimeData.CurrentAmmo == weaponConfig.RoundCapacity || weaponRuntimeData.State == WeaponState.RELOADING) 
        return;

        weaponRuntimeData.State = WeaponState.IDLE;
        
        _animator.SetBool("isFiring", false);
        weaponRuntimeData.State = WeaponState.RELOADING;
        _animator.SetTrigger("ReloadTrigger");
    }

    public override void HandleReloadEnd(){
        weaponRuntimeData.State = WeaponState.IDLE;
    }

    public override void HandleFiringAnimationEnd()
    {
        //no op
    }
    public override void ResetWeaponState(){
        //Weapon.isFiring = false;
        //Weapon.isReloading = false;
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = weaponConfig.sprite;
        
    }
}
