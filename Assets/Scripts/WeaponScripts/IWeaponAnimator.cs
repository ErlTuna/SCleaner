using System.Collections;

public interface IWeaponAnimator
{
    void StartPrimaryAttackAnim();
    void HandlePrimaryAttackAnimEnd();
    void StartReloadAnim();
    void HandleReloadAnimEnd();
    void ResetAnimParams();
    void SetBool(string name, bool value);
    void SetTrigger(string name);
    void SetFloat(string name, float value);
    void SetInt(string name, int value);
    IEnumerator WaitForAnimation(string stateName);
}
