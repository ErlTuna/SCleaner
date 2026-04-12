using System;
using UnityEngine;

public class ShellBasedWeaponAnimator : WeaponAnimatorBase, IShellBasedWeaponAnimator
{
    // Shell reload–specific events, called by animation event wrappers.
    public event Action OnShellInserted;

    public override void StartPrimaryAttackAnim()
    {
        SetBool("isReloading", false);
        SetBool("isFiring", true);
    }

    public override void LoopPrimaryAttackAnim()
    {
        SetBool("isFiring", true);
        SetBool("isTriggerHeld", true);
    }
    
    public override void StartReloadAnim()
    {
        ResetAnimParams();
        SetBool("isReloading", true);
        SetTrigger("ReloadStarted");
    }

    // Shell based weapons can loop their reload animations, called by the concrete weapon.
    public void LoopReload()
    {
        SetBool("isReloading", true);
        SetTrigger("LoopReload");
    }




    public override void AnimationEvent_BulletSpawnPointReached()
    {
        //Debug.Log("Animator component, bullet spawn point reached. Time : " + Time.time);
        OnBulletSpawnPointReached?.Invoke();
    }

    public override void AnimationEvent_PrimaryAttackAnimEnd()
    {
        //Debug.Log("Animator component, primary attack end point reached. Time : " + Time.time);
        SetBool("isFiring", false);
        OnAttackAnimEnd?.Invoke();
    }

    public override void AnimationEvent_HandleReloadAnimEnd()
    {
        OnReloadAnimEnd?.Invoke();
    }

    public void AnimationEvent_ShellInserted() => OnShellInserted?.Invoke();
}
