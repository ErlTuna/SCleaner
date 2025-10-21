using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Pattern", menuName = "ScriptableObjects/Attack Patterns/Burst Attack Pattern")]
public class BurstPattern : AttackPatternSO
{
    public int burstCount = 1;
    public float TimeBetweenShots = 0f;

    public override IEnumerator Execute(BaseEnemyWeapon weapon)
    {
        IsExecuting = true;
        yield return new WaitForSeconds(InitialDelay);
        for (int i = 0; i < burstCount; i++)
        {
            if (weapon.CanFire())
            {
                weapon.WeaponAnimator.SetTrigger("FireTrigger");
                weapon.WeaponAnimator.SetBool("isFiring", true);
                yield return weapon.WeaponAnimator.WaitForAnimation("Shooting");
                weapon.WeaponAnimator.SetBool("isFiring", false);
                yield return new WaitForSeconds(TimeBetweenShots);
            }
            else
            {
                break;
            }
        }


        cooldownCoroutine = weapon.StartCoroutine(Cooldown(weapon));
        weapon.WeaponAnimator.SetBool("isFiring", false);
        IsExecuting = false;
    }
    
    public override IEnumerator Cooldown(BaseEnemyWeapon weapon)
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        IsOnCooldown = false;
    }
}
