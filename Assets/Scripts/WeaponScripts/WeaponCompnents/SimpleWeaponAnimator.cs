using UnityEngine;

public class SimpleWeaponAnimator : WeaponAnimatorBase
{        
    public override void StartPrimaryAttackAnim()
    {
        SetBool("isFiring", true);
    }

    public override void LoopPrimaryAttackAnim()
    {
        SetBool("isFiring", true);
        SetBool("isTriggerHeld", true);
    }

    public override void StartReloadAnim()
    {
        SetBool("isFiring", false);
        SetBool("isTriggerHeld", false);

        SetBool("isReloading", true);
        SetTrigger("ReloadStarted");
        SetTrigger("ReloadTrigger");
    }

    // Animation Event callbacks
    public override void AnimationEvent_BulletSpawnPointReached()
    {
        OnBulletSpawnPointReached?.Invoke();
    }

    public override void AnimationEvent_PrimaryAttackAnimEnd() 
    {
        SetBool("isFiring", false);
        OnAttackAnimEnd?.Invoke();
    }

    public override void AnimationEvent_HandleReloadAnimEnd() 
    {
        SetBool("isReloading", false);
        OnReloadAnimEnd?.Invoke();
    }
}
