using UnityEngine;

public class BasicSMG : PlayerWeapon, IInstantFiringWeapon
{
    void OnEnable()
    {
        weaponAnimatorComponent.OnBulletSpawnPointReached += SpawnBullet;
        weaponAnimatorComponent.OnAttackAnimEnd += OnPrimaryAttackAnimEnd;
        weaponAnimatorComponent.OnReloadAnimEnd += OnReloadAnimEnd;
    }

    void OnDisable()
    {
        weaponAnimatorComponent.OnBulletSpawnPointReached -= SpawnBullet;
        weaponAnimatorComponent.OnAttackAnimEnd -= OnPrimaryAttackAnimEnd;
        weaponAnimatorComponent.OnReloadAnimEnd -= OnReloadAnimEnd;
    }

    void Update()
    {
        CheckAmmoStatus();
    }

    #region Overrides

    public override bool CanFire()
    {
        return PrimaryAttackStrategy && AmmoManager.HasAmmo() && WeaponRuntimeData.State == WeaponState.IDLE;
    }

    protected override void SpawnBullet()
    {   
        Quaternion spreadRot = CalculateBulletSpread();
        Vector2 trajectory = spreadRot * Vector2.right;
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletPrefab, FiringPoints[0].transform.position, Quaternion.identity);

        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, trajectory, WeaponRuntimeData, WeaponConfig);
        WeaponRuntimeData.TimeSinceLastFired = Time.unscaledTime;
        AmmoManager.UseAmmo();
    }    

    #region Input Handlers

    public override void HandlePrimaryAttackInput()
    {
        // Delegate firing logic to strategy
        if (CanFire())
            PrimaryAttackStrategy.OnTriggerPressed(this);

        else
        {
            weaponRuntimeData.isTriggerHeld = true;
        }
            
    }

    public override void HandlePrimaryAttackInputRelease()
    {
        if (weaponRuntimeData.isTriggerHeld == false) return;

        weaponRuntimeData.isTriggerHeld = false;

        if (PrimaryAttackStrategy)
            PrimaryAttackStrategy.OnTriggerReleased(this);
    }

    public override void HandleReloadInput()
    {
        if (AmmoManager.CanReload() && WeaponRuntimeData.State != WeaponState.RELOADING)
        {    
            ReloadContext reloadContext = CreateReloadContext();
            AmmoManager.SetReloadContext(reloadContext);

            SetState(WeaponState.RELOADING);
            WeaponAnimator.StartReloadAnim();
        }
    }

    #endregion

    #region Animation End Handlers

    protected override void OnPrimaryAttackAnimEnd()
    {
        // Spawn the bullet
        //SpawnBullet();

        // Let strategy decide what to do if trigger is held
        if (weaponRuntimeData.isTriggerHeld && CanContinuePrimaryAttack())
        {
            Debug.Log("Trigger held");
            PrimaryAttackStrategy.OnTriggerHeld(this);
        }
        else
        {
            SetState(WeaponState.IDLE);
        }
    }


    protected override void OnReloadAnimEnd()
    {
        AmmoManager.UseReloadStrategy();
        SetState(WeaponState.IDLE); 
    }

    #endregion

    #region IInstantFiringWeapon


    // Optional: called by strategy if weapon supports manual cancel
    public void OnAttackInputReleased()
    {
        if (WeaponRuntimeData.State == WeaponState.PRIMARY_ATTACK)
        {
            weaponAnimatorComponent.SetBool("isTriggerHeld", false);
            SetState(WeaponState.IDLE);
        }
    }

    #endregion

    #endregion
}
