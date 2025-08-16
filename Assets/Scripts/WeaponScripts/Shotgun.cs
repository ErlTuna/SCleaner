using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    public WeaponSO WeaponInfo { get; set; }
    private BulletSO bulletInfo;
    List<GameObject> bullets = new List<GameObject>();
    [SerializeField] GameObject[] firingPoints;
    [SerializeField] Animator _animator;
    [SerializeField] RuntimeAnimatorController _runtimeAnimatorController;
    bool _hasReloadStarted;
    
    void HandleBulletDestroyed(GameObject bullet){
        bullets.Remove(bullet);
    }

    void OnDestroy(){
        Bullet.OnBulletDestroyedEvent_v2 -= HandleBulletDestroyed;
    }

    void Awake(){
        Bullet.OnBulletDestroyedEvent_v2 += HandleBulletDestroyed;

        if (WeaponInfo == null){
            //Debug.Log("Shotgun weapon info missing. Loading resource.");
            WeaponInfo = Resources.Load<WeaponSO>("ScriptableObjects/ShotgunSO");
        }
        
        if(WeaponInfo.bulletInfo == null){
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) missing. Loading shotgun bullet info resource.");
            bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/ShotgunBulletInfo");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else {
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) already exists.");
            bulletInfo = WeaponInfo.bulletInfo;
        }
        
        if (!_animator){
            _animator = GetComponent<Animator>();
        }

        if (_animator.runtimeAnimatorController == null){
            print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Shotgun_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
        WeaponInfo.Init();
    }

    void Update(){
        if(!HasAmmo() && WeaponInfo.currentReserveAmmo != 0 && !WeaponInfo.isReloading){
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && WeaponInfo.currentReserveAmmo == 0 && !WeaponInfo.isReloading){
            HandlePrimaryAttackInputCancel();
        }
    }
    public void PrimaryAttack()
    {
        GameObject overlappingEnemy = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, enemyLayer);
        if(overlappingEnemy != null){
            IDamageable damageable = overlappingEnemy.GetComponent<IDamageable>();
            damageable?.TakeDamage(WeaponInfo.damage * WeaponInfo.pelletCount);
        }
        else{
            for (int i = 0; i < firingPoints.Length; ++i){
            GameObject bullet = Instantiate(WeaponInfo.bulletPrefab, firingPoints[i].transform.position, firingPoints[i].transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetupBulletParameters(bulletInfo.projectileSpeed, bulletInfo.size, WeaponInfo.damage, bulletInfo.lifeTime);
            bullets.Add(bullet);
            }
        }
        IWeapon.Invoke();
        WeaponInfo.currentAmmo--;
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }
    
    public bool HasAmmo(){
        if (WeaponInfo.currentAmmo == 0){
            return false;
        }
        return true;
    }

    public void Reload()
    {   
        
        ++WeaponInfo.currentAmmo;
        --WeaponInfo.currentReserveAmmo;
        
        if(WeaponInfo.currentAmmo < WeaponInfo.roundCapacity){
            _animator.SetTrigger("ReloadLoopTrigger");
            return;
        }
        HandleReloadEnd();
    }

    public void HandlePrimaryAttackInput(){
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        //if(ObstructionChecker.CheckMuzzleEnvironmentOverlap(muzzleTipCheck, environmentLayers)) return;

        if(!HasAmmo()) return;
        if(WeaponInfo.isFiring) return;

        if(_hasReloadStarted && HasAmmo() || WeaponInfo.isReloading && HasAmmo()){
            _hasReloadStarted = false;
            WeaponInfo.isReloading = false;
            _animator.SetBool("hasReloadStarted", false);
            _animator.ResetTrigger("ReloadLoopTrigger");
        }

        //_animator.SetBool("hasReloaded", false);
        WeaponInfo.isFiring = true;
        _animator.SetBool("isFiring", true);
    }

    public void HandlePrimaryAttackInputCancel(){
        WeaponInfo.isFiring = false;
        _animator.SetBool("isFiring", false);
    }
    public void HandleFiringAnimationEnd(){

    }

    public void HandleReloadStart()
    {
        if(WeaponInfo.currentReserveAmmo == 0 || WeaponInfo.currentAmmo == WeaponInfo.roundCapacity || WeaponInfo.isReloading) 
        return;
        
        WeaponInfo.isFiring = false;
        _animator.SetBool("isFiring", false);

        _animator.SetBool("hasReloaded", false);
        _hasReloadStarted = true;
        _animator.SetBool("hasReloadStarted", true);
       
    }

    public void HandleReloadLoopStart(){

        if(!WeaponInfo.isReloading){
        _animator.SetBool("hasReloadStarted", false);
        WeaponInfo.isReloading = true;
        _animator.SetBool("isReloading", true);
        }

    }

    public void HandleReloadStartEnd(){
        _animator.SetBool("hasReloadStarted", false);
    }

    public void HandleReloadEnd()
    {
        WeaponInfo.isReloading = false;
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloaded", true);
    }

    public void ResetWeaponState(){
        WeaponInfo.isFiring = false;
        WeaponInfo.isReloading = false;
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloadStarted", false);
        _animator.SetBool("hasReloaded", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.ResetTrigger("ReloadLoopTrigger");
        _animator.ResetTrigger("ReloadCancelTrigger");
        //_animator.Play("Idle");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = WeaponInfo.sprite;
        
    }
}
