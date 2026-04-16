using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Revive Effect")]
public class ReviveEffectSO : EffectSO
{
    public int healAmount;
    public bool singleUse;

    public override IPersistentEffect CreateEffectInstance()
    {
        return new ReviveEffect(healAmount, singleUse);
    }
}
