using UnityEngine;

public class EnemyRevolver : BaseEnemyWeapon
{
    
    [SerializeField] Unit _ownerScript;

    protected override void Awake()
    {
        base.Awake();
        WeaponRuntime runtimeData = WeaponRuntimeFactory.Create(weaponConfig);
        InitializeWithRuntimeData(runtimeData);
    }

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

    protected override void SpawnBullet()
    {
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletPrefab, FiringPoints[0].transform.position, Quaternion.identity);
        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, FiringPoints[0].transform.right, WeaponRuntimeData, WeaponConfig);
        AmmoManager.UseAmmo();
    }

    protected override void OnPrimaryAttackAnimEnd()
    {
        if (AmmoManager.HasAmmo() == false && AmmoManager.CanReload())
        {   
            ReloadContext reloadContext = CreateReloadContext();
            AmmoManager.SetReloadContext(reloadContext);

            SetState(WeaponState.RELOADING);
            WeaponAnimator.StartReloadAnim();
        }

        else
            SetState(WeaponState.IDLE);
    }

    protected override void OnReloadAnimEnd()
    {
        Debug.Log("Reload anim ended for enemy revolver.");
        AmmoManager.UseReloadStrategy();
        SetState(WeaponState.IDLE); 
    }
}
