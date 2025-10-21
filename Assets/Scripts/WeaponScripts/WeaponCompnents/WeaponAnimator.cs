using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


// Responsible for handling a weapon's animations.
// Starting animations and their endings are handled here.

public class WeaponAnimator : MonoBehaviour, IWeaponAnimator
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
        ResetAnimParams();
        _owner.OnAttackAnimEnd();

    }
    public void StartReloadAnim()
    {
        ResetAnimParams();
        SetBool("isReloading", true);
        SetTrigger("ReloadStarted");
    }

    public virtual void HandleReloadAnimEnd()
    {
        _owner.OnReloadAnimEnd();
    }

    public virtual void ResetAnimParams()
    {
        _paramTable.ResetParameters(animator);
    }
    public void SetBool(string name, bool value)
    {
        if (_paramTable == null)
        {
            Debug.Log("Param table missing.");
            return;
        }

        if (animator == null)
        {
            Debug.Log("Animator is null");
            return;
        }

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
        if (_paramTable == null)
        {
            Debug.Log("Param table missing.");
            return;
        }

        if (animator == null)
        {
            Debug.Log("Animator is null");
            return;
        }

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
        if (_paramTable == null)
        {
            Debug.Log("Param table missing.");
            return;
        }

        if (animator == null)
        {
            Debug.Log("Animator is null");
            return;
        }

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

        if (_paramTable == null)
        {
            Debug.Log("Param table missing.");
            return;
        }

        if (animator == null)
        {
            Debug.Log("Animator is null");
            return;
        }

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
        if (_paramTable == null)
        {
            Debug.Log("Param table missing.");
            return;
        }

        if (animator == null)
        {
            Debug.Log("Animator is null");
            return;
        }
        
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

    public IEnumerator WaitForAnimation(string stateName)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        Debug.Log("Is it over?..");
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                                         animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        );
    }
}






