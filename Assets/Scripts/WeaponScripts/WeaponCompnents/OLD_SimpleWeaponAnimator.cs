using UnityEngine;

public class OLD_SimpleWeaponAnimator : WeaponAnimatorBase
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
    }

    // called by Animation Event State Behaviour
    public override void AnimationEvent_BulletSpawnPointReached()
    {
        OnBulletSpawnPointReached?.Invoke();
    }

    // called by Animation Event State Behaviour
    public override void AnimationEvent_PrimaryAttackAnimEnd() 
    {
        SetBool("isFiring", false);
        OnAttackAnimEnd?.Invoke();
    }

    // called by Animation Event State Behaviour
    public override void AnimationEvent_HandleReloadAnimEnd() 
    {
        SetBool("isReloading", false);
        OnReloadAnimEnd?.Invoke();
    }
}
