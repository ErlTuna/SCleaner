using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;


// Parameter Table to hold the parameters an Animator may have
// The backing list is populated using the inspector (SerializeReferenceEditor helps)
// Afterwards, the list is used to build a dictionary
// Dictionary uses the Hash as Key to return AnimatorParameter objects


[CreateAssetMenu(fileName = "Animator Param Table", menuName = "ScriptableObjects/Animator/Parameter Table")]
public class AnimatorParamTableSO : ScriptableObject
{
    [SerializeReference, SR]
    List<AnimatorParameter> _parameterList = new();
    Dictionary<int, AnimatorParameter> _parameterDict;

    // Ensuring the Dictionary is read-only to the outside.
    public IReadOnlyDictionary<int, AnimatorParameter> Parameters => _parameterDict;


    void OnEnable()
    {
        BuildParameterDictionary();
    }

    // Returns an AnimatorParameter using its name
    // The name is used to calculate the hash
    public AnimatorParameter GetByName(string name)
    {
        Debug.Log("Param name is : " + name);
        int hash = Animator.StringToHash(name);
        //Debug.Log("Hash is : " + hash);
        return GetByHash(hash);
    }


    // Returns an AnimatorParameter using its hash directly
    public AnimatorParameter GetByHash(int hash)
    {
        return _parameterDict.TryGetValue(hash, out AnimatorParameter param) ? param : null;
    }

    // Using hash values, resets ALL parameters in the given Animator 
    public void ResetParameters(Animator animator)
    {

        // cache the available parameters in a hashset
        // (hashset avoids duplicates)
        HashSet<int> availableParams = new();

        // animator.parameters returns an array of parameters in an animator
        foreach (var p in animator.parameters)
        {
            availableParams.Add(Animator.StringToHash(p.name));
        }

        foreach (var param in _parameterDict.Values)
        {
            // Skip if parameter not defined in current animator
            // This avoids warnings where a given parameter may not exist
            // when using the same table for similar weapons that have
            // small differences
            if (!availableParams.Contains(param.Hash))
                continue; 


            switch (param.Type)
            {
                case AnimatorParamType.Bool:
                    animator.SetBool(param.Hash, false);
                    break;
                case AnimatorParamType.Trigger:
                    animator.ResetTrigger(param.Hash);
                    break;
                case AnimatorParamType.Int:
                    animator.SetInteger(param.Hash, 0);
                    break;
                case AnimatorParamType.Float:
                    animator.SetFloat(param.Hash, 0f);
                    break;
                case AnimatorParamType.None:
                    Debug.LogWarning($"Parameter '{param.Name}' has type 'None'.");
                    break;
            }
        }
    }

    // Builds the AnimatorParameter dictionary using the backing list
    // as Unity and SerializeReferenceEditor can't serialize dictionaries into the inspector.
    // List items are added through the inspector using SerializeReferenceEditor.

    void BuildParameterDictionary()
    {
        _parameterDict = new Dictionary<int, AnimatorParameter>();

        foreach (AnimatorParameter param in _parameterList)
        {
            if (param == null || string.IsNullOrEmpty(param.Name))
                continue;

            int hash = param.Hash;

            if (!_parameterDict.ContainsKey(hash))
                _parameterDict.Add(hash, param);
            else
                Debug.LogWarning($"Duplicate animator parameter: '{param.Name}' in {name}");
        }
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        //BuildParameterDictionary();
    }
    #endif
}
