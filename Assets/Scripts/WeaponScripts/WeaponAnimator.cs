using UnityEngine;


// Responsible for handling a weapon's animations.
// Starting animations and their endings are handled here.

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] BaseWeapon _owner;
    [SerializeField] AnimatorParamTableSO _paramTable;
    public Animator animator;


    public void StartPrimaryAttackAnim()
    {
        SetBool("isFiring", true);
        SetTrigger("FireTrigger");
    }

    public virtual void HandlePrimaryAttackAnimEnd()
    {
        //Debug.Log("Primary attack end handler called");
        ResetAnimParams();
        _owner.PrimaryAttackStrategy.HandleAttackEnd(_owner);

    }
    public void StartReloadAnim()
    {
        //Debug.Log("Reload anim started");
        ResetAnimParams();
        SetBool("isReloading", true);
        SetTrigger("ReloadStarted");
    }

    public virtual void HandleReloadAnimEnd()
    {
        _owner.AmmoManager.UseReloadStrategy();
        _owner.AmmoManager.ShouldContinueReloading();
    }

    public virtual void ResetAnimParams()
    {
        _paramTable.ResetParameters(animator);
    }
    public void SetBool(string name, bool value)
    {
        var param = _paramTable.GetByName(name);

        if (param == null)
        {
            //Debug.Log("Given parameter does not exist");
            return;
        }

        if (param.Type != AnimatorParamType.Bool)
        {
            //Debug.LogWarning($"Animator parameter '{name}' is not of type Bool.");
            return;
        }

        animator.SetBool(param.Hash, value);
    }

    public void SetTrigger(string name)
    {
        var param = _paramTable.GetByName(name);

        if (param == null)
        {
            //Debug.Log("Given parameter does not exist");
            return;
        }

        if (param.Type != AnimatorParamType.Trigger)
        {
            //Debug.LogWarning($"Animator parameter '{name}' is not of type Trigger.");
            return;
        }

        animator.SetTrigger(param.Hash);
    }

    public void ResetTrigger(string name)
    {
        var param = _paramTable.GetByName(name);

        if (param == null)
        {
            //Debug.Log("Given parameter does not exist");
            return;
        }

        if (param.Type != AnimatorParamType.Trigger)
        {
            //Debug.LogWarning($"Animator parameter '{name}' is not of type Trigger.");
            return;
        }

        animator.ResetTrigger(param.Hash);
    }

    public void SetInt(string name, int value)
    {
        var param = _paramTable.GetByName(name);

        if (param == null)
        {
            //Debug.Log("Given parameter does not exist");
            return;
        }

        if (param.Type != AnimatorParamType.Int)
        {
            //Debug.LogWarning($"Animator parameter '{name}' is not of type Int.");
            return;
        }

        animator.SetInteger(param.Hash, value);
    }

    public void SetFloat(string name, float value)
    {
        var param = _paramTable.GetByName(name);

        if (param == null)
        {
            //Debug.Log("Given parameter does not exist");
            return;
        }

        if (param.Type != AnimatorParamType.Float)
        {
            //Debug.LogWarning($"Animator parameter '{name}' is not of type Float.");
            return;
        }

        animator.SetFloat(param.Hash, value);
    }
}






