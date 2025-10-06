using System;
using UnityEngine;


// Wrapper class for Animator parameters
// Serializable for the purpose of being used with SerializeReferenceEditor

[Serializable]
public class AnimatorParameter
{
    public string Name;
    public AnimatorParamType Type;

    // nullable int for lazy initialization for when the hash is accessed for the first time
    private int? _cachedHash;

    // if the hash is accessed for the first time (isn't cached), cache it and return it.
    // Unity uses CRC32 to generate a hash using a string.
    public int Hash => _cachedHash ??= Animator.StringToHash(Name);

    //default constructor to use with SerializeReferenceEditor
    public AnimatorParameter()
    {
        Name = "none";
        Type = AnimatorParamType.None;
    }

    //sets the value of AN Animator's parameter using AnimatorParameter object
    //if a given record exists in the dictionary
    /*public void SetValue(Animator animator, object value = null)
    {
        if (animator == null || string.IsNullOrEmpty(Name))
        {
            Debug.LogWarning("Animator or parameter name is null/empty.");
            return;
        }

        switch (Type)
        {
            case AnimatorParamType.Bool:
                if (value is bool boolVal)
                    animator.SetBool(Hash, boolVal);
                else
                    Debug.LogWarning($"Expected bool for Animator parameter {Name}");
                break;

            case AnimatorParamType.Int:
                if (value is int intVal)
                    animator.SetInteger(Hash, intVal);
                else
                    Debug.LogWarning($"Expected int for Animator parameter {Name}");
                break;

            case AnimatorParamType.Float:
                if (value is float floatVal)
                    animator.SetFloat(Hash, floatVal);
                else
                    Debug.LogWarning($"Expected float for Animator parameter {Name}");
                break;

            case AnimatorParamType.Trigger:
                animator.SetTrigger(Hash);
                break;

            case AnimatorParamType.None:
                Debug.LogWarning("Animator parameter type is None.");
                break;

            default:
                Debug.LogWarning($"Unhandled AnimatorParamType: {Type}");
                break;
        }
    }
    */
}
