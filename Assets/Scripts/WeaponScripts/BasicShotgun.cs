using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotgun : BaseWeapon
{
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    private BulletSO bulletInfo;
    List<GameObject> bullets = new List<GameObject>();
    [SerializeField] Transform[] firingPoints;
    bool _hasReloadStarted;
    
    void HandleBulletDestroyed(GameObject bullet){
        bullets.Remove(bullet);
    }

    void OnDestroy(){
        Bullet.OnBulletDestroyedEvent_v2 -= HandleBulletDestroyed;
    }

    void Awake(){
        Bullet.OnBulletDestroyedEvent_v2 += HandleBulletDestroyed;

        if (data.bulletData == null){
            //Debug.Log("Shotgun weapon info missing. Loading resource.");
            data = Resources.Load<WeaponData>("ScriptableObjects/ShotgunSO");
        }
        
        if(data.bulletData == null){
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) missing. Loading shotgun bullet info resource.");
            bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/ShotgunBulletInfo");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else {
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) already exists.");
            bulletInfo = data.bulletData;
        }
        
        if (!_animator){
            _animator = GetComponent<Animator>();
        }

        if (_animator.runtimeAnimatorController == null){
            print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Shotgun_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
    }

    void Update(){
        if(!HasAmmo() && data.currentReserveAmmo != 0 && data.state != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && data.currentReserveAmmo == 0 && data.state != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
        }
    }
    public override void PrimaryAttack()
    {
        GameObject overlappingEnemy = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, enemyLayer);
        if (overlappingEnemy != null)
        {
            IDamageable damageable = overlappingEnemy.GetComponent<IDamageable>();
            damageable?.TakeDamage(data.damage* data.pelletCount);
        }
        else
        {
            for (int i = 0; i < firingPoints.Length; ++i)
            {
                GameObject bullet = Instantiate(data.bulletData.bulletPrefab, firingPoints[i].transform.position, firingPoints[i].transform.rotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.SetupBulletParameters(bulletInfo.projectileSpeed, bulletInfo.size, data.damage, bulletInfo.lifeTime);
                bullets.Add(bullet);
            }
        }

        data.currentAmmo--;
        //IWeapon.InvokeOnWeaponFireEvent(this);
        //IWeapon.InvokeOnWeaponFiredVisuals();
    }

    void OnDrawGizmosSelected(){
        //Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        //Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }
    

    public override void Reload()
    {   
        
        ++data.currentAmmo;
        --data.currentReserveAmmo;
        
        if(data.currentAmmo < data.roundCapacity){
            _animator.SetTrigger("ReloadLoopTrigger");
            return;
        }
        HandleReloadEnd();
    }

    public override void HandlePrimaryAttackInput(){
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        //if(ObstructionChecker.CheckMuzzleEnvironmentOverlap(muzzleTipCheck, environmentLayers)) return;

        if(!HasAmmo()) return;
        if(data.state == WeaponState.FIRING) return;

        if(_hasReloadStarted && HasAmmo() || data.state == WeaponState.RELOADING && HasAmmo()){
            _hasReloadStarted = false;

            //Weapon.isReloading = false;
            data.state = WeaponState.IDLE;
            _animator.SetBool("hasReloadStarted", false);
            _animator.ResetTrigger("ReloadLoopTrigger");
        }

        //_animator.SetBool("hasReloaded", false);
        data.state = WeaponState.FIRING;
        _animator.SetBool("isFiring", true);
    }

    public override void HandlePrimaryAttackInputCancel(){
        data.state = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
    }
    public override void HandleFiringAnimationEnd(){

    }

    public override void HandleReloadStart()
    {
        if(data.currentReserveAmmo == 0 || data.currentAmmo == data.roundCapacity || data.state == WeaponState.RELOADING) 
        return;
        
        data.state = WeaponState.RELOADING;
        _animator.SetBool("isFiring", false);

        _animator.SetBool("hasReloaded", false);
        _hasReloadStarted = true;
        _animator.SetBool("hasReloadStarted", true);
       
    }

    public void HandleReloadLoopStart(){

        if(data.state != WeaponState.RELOADING){
        _animator.SetBool("hasReloadStarted", false);
        data.state = WeaponState.RELOADING;
        _animator.SetBool("isReloading", true);
        }

    }

    public void HandleReloadStartEnd(){
        _animator.SetBool("hasReloadStarted", false);
    }

    public override void HandleReloadEnd()
    {
        data.state = WeaponState.IDLE;
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloaded", true);
        //IWeapon.InwokeOnWeaponReloadEvent(this);
    }

    public override void ResetWeaponState(){
        //Weapon.isFiring = false;
        //Weapon.isReloading = false;
        data.state = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloadStarted", false);
        _animator.SetBool("hasReloaded", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.ResetTrigger("ReloadLoopTrigger");
        _animator.ResetTrigger("ReloadCancelTrigger");
        //_animator.Play("Idle");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        
    }
}
