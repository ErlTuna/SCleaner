using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Pattern", menuName = "ScriptableObjects/Attack Patterns/Burst Attack Pattern")]
public class BurstPattern : AttackPatternSO
{
    public int BurstCount = 0;
    public int ShotsPerBurst = 0;
    public float TimeBetweenShots = 0f;

    public override IEnumerator Execute(BaseEnemyWeapon weapon)
    {
        Debug.Log("Executing attack pattern.");
        Debug.Log("What is the wait time?" + TimeBetweenShots);

        IsExecuting = true;
        yield return new WaitForSeconds(InitialDelay);

        for (int currentBurst = 0; currentBurst < BurstCount; ++currentBurst)
        {
            for (int remainingShots = ShotsPerBurst; 0 < remainingShots; --remainingShots)
            {
                weapon.RequestFire();
                if (weapon.AmmoManager.HasAmmo() == false)
                    yield break;
                
                yield return new WaitUntil(() => weapon.CanFire());

                if (TimeBetweenShots > 0)
                {
                    yield return new WaitForSeconds(TimeBetweenShots);
                    
                }

                else
                {
                    Debug.Log("Looping fire.");
                    weapon.LoopFire();
                }

            }
        }

        cooldownCoroutine = weapon.StartCoroutine(Cooldown(weapon));
        IsExecuting = false;
    }
    
    public override IEnumerator Cooldown(BaseEnemyWeapon weapon)
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        IsOnCooldown = false;
    }
}
