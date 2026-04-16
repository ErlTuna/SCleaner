using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract IPersistentEffect CreateEffectInstance();
}
