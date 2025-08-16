using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StarterPistolScript : MonoBehaviour, IWeapon
{
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    public WeaponSO WeaponInfo { get; set;}
    private BulletSO bulletInfo;
    [SerializeField] Transform firingPoint;
    [SerializeField] Animator _animator;
    [SerializeField] RuntimeAnimatorController _runtimeAnimatorController;

    void Awake(){
        if (WeaponInfo == null){
            //Debug.Log("Pistol weapon info missing. Loading resource.");
            WeaponInfo = Resources.Load<WeaponSO>("ScriptableObjects/PistolSO");
        }
         
        if(WeaponInfo.bulletInfo == null){
            //Debug.Log("Pistol bullet info (part of pistol weapon info) missing. Loading pistol bullet info resource.");
            bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/PistolBulletSO");
        }
        else {
            //Debug.Log("Pistol bullet info (part of pistol bullet info) already exists.");
            bulletInfo = WeaponInfo.bulletInfo;
        }

        if (firingPoint == null){
            //Debug.Log("FiringPoint not assigned. Finding...");
            firingPoint = GetComponentInChildren<Transform>();   
        }

        if (_animator == null){
            _animator = GetComponent<Animator>();
        }
        
        if (_animator.runtimeAnimatorController == null){
            //print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Pistol_AC");
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

    public void ReceiveFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
    }


    public void PrimaryAttack(){
        
        if (!HasAmmo() || WeaponInfo.isReloading) return;
        
        //GameObject overlappingEnemy = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, enemyLayer);
        //GameObject overlap = ObstructionChecker.CheckMuzzleOverlapWithLine(rayCastEndPoint.transform, muzzleTipCheck.transform, enemyLayer, environmentLayers);
        
        /*if(overlap != null){
            print("Overlapped with something " + overlap);
            IDamageable damageable = overlap.GetComponent<IDamageable>();
            damageable?.TakeDamage(WeaponInfo.damage);
        }*/

        
        //print("Bullet instantiated");
        Bullet instantiatedBullet = Instantiate(WeaponInfo.bulletPrefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(bulletInfo.projectileSpeed, bulletInfo.size, WeaponInfo.damage, bulletInfo.lifeTime);
        
        IWeapon.Invoke();
        --WeaponInfo.currentAmmo;
    }
    public bool HasAmmo(){
        if (WeaponInfo.currentAmmo > 0){
            return true;
        }
        return false;
    }

    public void Reload()
    {
        int ammoBeforeReload = WeaponInfo.currentAmmo;
        WeaponInfo.currentAmmo = 0;
        WeaponInfo.isReloading = true;
        
        if(WeaponInfo.roundCapacity - ammoBeforeReload <= WeaponInfo.currentReserveAmmo){
            WeaponInfo.currentReserveAmmo -= WeaponInfo.roundCapacity - ammoBeforeReload;
            WeaponInfo.currentAmmo = WeaponInfo.roundCapacity;
        }
        else{
            WeaponInfo.currentAmmo += WeaponInfo.currentReserveAmmo;
            WeaponInfo.currentReserveAmmo = 0;
        }

        WeaponInfo.isReloading = false;
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }

    public void HandlePrimaryAttackInput()
    {
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        //if(ObstructionChecker.CheckMuzzleEnvironmentOverlap(muzzleTipCheck, environmentLayers)) return;
        if(!HasAmmo() || WeaponInfo.isReloading || WeaponInfo.isFiring) return;

        IWeapon.Invoke();
        WeaponInfo.isFiring = true;
        _animator.SetBool("isFiring", true);
    }

    public void HandlePrimaryAttackInputCancel(){
        WeaponInfo.isFiring = false;
        _animator.SetBool("isFiring", false);
    }

    public void HandleReloadStart(){

        if(WeaponInfo.currentReserveAmmo == 0 || WeaponInfo.currentAmmo == WeaponInfo.roundCapacity || WeaponInfo.isReloading) 
        return;

        WeaponInfo.isFiring = false;
        
        _animator.SetBool("isFiring", WeaponInfo.isFiring);
        WeaponInfo.isReloading = true;
        _animator.SetTrigger("ReloadTrigger");
    }

    public void HandleReloadEnd(){
        WeaponInfo.isReloading = false;
    }

    public void HandleFiringAnimationEnd()
    {
        //no op
    }
    public void ResetWeaponState(){
        //print("Called reset for pistol");
        WeaponInfo.isFiring = false;
        WeaponInfo.isReloading = false;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("ReloadTrigger");
        //_animator.Play("Idle");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = WeaponInfo.sprite;
        
    }
}
