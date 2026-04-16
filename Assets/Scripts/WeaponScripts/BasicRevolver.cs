using UnityEngine;

public class BasicRevolver : PlayerWeapon, IChargeFiringWeapon
{
    IChargeBasedWeaponAnimator _chargeAnimator;

    void OnEnable()
    {
        _chargeAnimator = weaponAnimatorComponent as IChargeBasedWeaponAnimator;

        weaponAnimatorComponent.OnAttackAnimEnd += OnPrimaryAttackAnimEnd;
        weaponAnimatorComponent.OnBulletSpawnPointReached += SpawnBullet;
        weaponAnimatorComponent.OnReloadAnimEnd += OnReloadAnimEnd;

        _chargeAnimator.OnChargeCompleted += PerformAttack;
        _chargeAnimator.OnChargeCanceled += CancelCharge;
    }

    void OnDisable()
    {
        weaponAnimatorComponent.OnAttackAnimEnd -= OnPrimaryAttackAnimEnd;
        weaponAnimatorComponent.OnBulletSpawnPointReached -= SpawnBullet;
        weaponAnimatorComponent.OnReloadAnimEnd -= OnReloadAnimEnd;

        _chargeAnimator.OnChargeCompleted -= PerformAttack;
        _chargeAnimator.OnChargeCanceled -= CancelCharge;
    }

    void Update()
    {
        CheckAmmoStatus();
    }

    // --- Input forwarding ---

    public override void HandlePrimaryAttackInput()
    {
        if (CanCharge())
        {
            PrimaryAttackStrategy.OnTriggerPressed(this);
        }

        // if (WeaponRuntimeData.State == WeaponState.CHARGING_PRIMARY_ATTACK)
        else 
        {
            PrimaryAttackStrategy.OnTriggerHeld(this);
            weaponRuntimeData.isTriggerHeld = true;
        }
    }

    public override void HandlePrimaryAttackInputRelease()
    {
        if (weaponRuntimeData.isTriggerHeld == false) return;

        weaponRuntimeData.isTriggerHeld = false;

        if (WeaponRuntimeData.State == WeaponState.CHARGING_PRIMARY_ATTACK)
        {
            PrimaryAttackStrategy.OnTriggerReleased(this);
        }
        
    }

    public override void HandleReloadInput()
    {
        if (AmmoManager.CanReload() && WeaponRuntimeData.State != WeaponState.RELOADING)
        {
            var reloadContext = CreateReloadContext();
            AmmoManager.SetReloadContext(reloadContext);

            SetState(WeaponState.RELOADING);
            WeaponAnimator.StartReloadAnim();
        }
    }

    // --- IChargeFiringWeapon Implementation ---

    public void BeginCharge()
    {
        SetState(WeaponState.CHARGING_PRIMARY_ATTACK);
        _chargeAnimator.StartChargeAnimation();
    }

    public void PerformAttack()
    {
        SetState(WeaponState.PRIMARY_ATTACK);
        weaponAnimatorComponent.StartPrimaryAttackAnim();
    }

    public void CancelCharge()
    {
        _chargeAnimator.CancelChargeAnimation();
        SetState(WeaponState.IDLE);
    }

    public void OnAttackInputReleased()
    {
        if (WeaponRuntimeData.State == WeaponState.CHARGING_PRIMARY_ATTACK)
        {
            _chargeAnimator.CancelChargeAnimation();
        }
    }

    public bool CanCharge()
    {
        return PrimaryAttackStrategy && WeaponRuntimeData.State == WeaponState.IDLE && AmmoManager.HasAmmo();
    }

    // --- Animation End Handlers ---

    protected override void OnPrimaryAttackAnimEnd()
    {
        if (weaponRuntimeData.isTriggerHeld && CanFire())
            PrimaryAttackStrategy.OnTriggerPressed(this);
        else
            SetState(WeaponState.IDLE);
    }

    protected override void OnReloadAnimEnd()
    {
        Debug.Log("reload anim end called");
        AmmoManager.UseReloadStrategy();
        SetState(WeaponState.IDLE);
    }

    protected override void SpawnBullet()
    {
        Debug.Log("Bang!!");
        GameObject bullet = Instantiate(WeaponConfig.BulletConfig.Prefab, FiringPoints[0].position, transform.rotation);
        bullet.GetComponent<Bullet>().Initialize(gameObject, FiringPoints[0].right, WeaponRuntimeData, WeaponConfig);
        AmmoManager.UseAmmo();
    }
}

