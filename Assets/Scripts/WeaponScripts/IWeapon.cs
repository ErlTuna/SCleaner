using System;

public interface IWeapon 
{
    public static event Action OnWeaponFireEvent;
    public static void Invoke() => OnWeaponFireEvent?.Invoke();
    WeaponSO WeaponInfo{get;set;}
    public void Reload();
    public void HandlePrimaryAttackInput();
    public void HandlePrimaryAttackInputCancel();
    public void HandleFiringAnimationEnd();
    public void HandleReloadStart();
    public void HandleReloadEnd();
    public void PrimaryAttack();
    public bool HasAmmo();
    public void ResetWeaponState();
    
}
