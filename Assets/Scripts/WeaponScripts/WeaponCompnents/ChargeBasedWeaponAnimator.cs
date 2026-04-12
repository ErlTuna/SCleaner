using System;

public class ChargeBasedWeaponAnimator : WeaponAnimatorBase, IChargeBasedWeaponAnimator
{
    public event Action OnChargeCompleted;
    public event Action OnChargeCanceled;


    // These are called by Animation Events
    public override void AnimationEvent_BulletSpawnPointReached() => OnBulletSpawnPointReached?.Invoke();
    public void AnimationEvent_ChargeCompleted() => OnChargeCompleted?.Invoke();

    public override void AnimationEvent_HandleReloadAnimEnd()
    {
        SetBool("isReloading", false);
        OnReloadAnimEnd?.Invoke();
        
    }

    public override void AnimationEvent_PrimaryAttackAnimEnd()
    {
        SetBool("isCharging", false);
        OnAttackAnimEnd?.Invoke();
    }

    public override void StartPrimaryAttackAnim()
    {
        SetTrigger("FireTrigger");
    }

    public override void StartReloadAnim()
    {
        SetBool("isCharging", false);
        SetTrigger("ReloadTrigger");
        SetBool("isReloading", true);

    }

    public override void LoopPrimaryAttackAnim()
    {
        //ResetAnimParams();
        //SetBool("isFiring", true);
        //SetBool("isTriggerHeld", true);
    }

    public void StartChargeAnimation()
    {
        SetBool("isCharging", true);
    }

    public void CancelChargeAnimation()
    {
        SetBool("isCharging", false);
    }
}
