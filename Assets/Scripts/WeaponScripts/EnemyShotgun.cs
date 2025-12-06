using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : BaseEnemyWeapon
{
    [SerializeField] Unit _ownerScript;
    Coroutine _attackPatternCoroutine;
    List<GameObject> _bullets = new();

    public override void ExecuteAttackPattern()
    {
        if (AttackPattern == null)
        {
            Debug.Log("Attack pattern missing");
            return;
        } 

        if (AmmoManager.HasAmmo() && CanFire() && AttackPattern.IsOnCooldown != true && AttackPattern.IsExecuting != true)
        {
            _attackPatternCoroutine = StartCoroutine(AttackPattern.Execute(this));
        }
            
    }

    public override void SpawnBullet()
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

    public override void OnAttackAnimEnd()
    {
        if (AmmoManager.HasAmmo() != true && AmmoManager.CanReload())
            AmmoManager.HandleReloadStart();
    }

    public override void OnReloadAnimEnd()
    {
        AmmoManager.UseReloadStrategy();
        AmmoManager.ShouldContinueReloading();
    }
}
