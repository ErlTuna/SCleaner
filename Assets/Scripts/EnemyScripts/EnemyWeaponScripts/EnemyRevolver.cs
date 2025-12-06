using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolver : BaseEnemyWeapon
{
    
    [SerializeField] Unit _ownerScript;
    Coroutine _attackPatternCoroutine;

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
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletPrefab, FiringPoints[0].transform.position, Quaternion.identity);
        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, FiringPoints[0].transform.right, WeaponRuntimeData, WeaponConfig);
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
