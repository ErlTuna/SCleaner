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
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletData.Prefab, FiringPoints[0].transform.position, FiringPoints[0].transform.rotation);
        instantiatedBullet.GetComponent<Bullet>().Setup(gameObject, WeaponRuntimeData, WeaponConfig);
        instantiatedBullet.SetActive(true);
        AmmoManager.UseAmmo();
        WeaponRuntimeData.TimeSinceLastFired = Time.unscaledTime;
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
