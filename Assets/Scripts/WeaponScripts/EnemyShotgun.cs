using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : BaseEnemyWeapon
{
    [SerializeField] Unit _ownerScript;
    List<GameObject> _bullets = new();

    protected override void Awake()
    {
        base.Awake();
        WeaponRuntimeData runtimeData = WeaponRuntimeFactory.Create(weaponConfig);
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
        BulletConfigSO bulletData;
        for (int i = 0; i < FiringPoints.Length; ++i)
        {
            bulletData = WeaponConfig.BulletConfig;
            GameObject bullet = Instantiate(bulletData.Prefab, FiringPoints[i].transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(gameObject, FiringPoints[i].transform.right, WeaponRuntimeData, WeaponConfig, i);
            _bullets.Add(bullet);
        }

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
        Debug.Log("Reload anim ended for enemy shotgun.");
        AmmoManager.UseReloadStrategy();
        SetState(WeaponState.IDLE); 
    }

    //public override bool CanFire() => AmmoManager.HasAmmo();
}
