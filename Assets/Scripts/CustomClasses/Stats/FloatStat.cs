using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatStat
{
    public float BaseValue {get; private set;}
    readonly List<FloatModifier> _modifiers = new();
    float _cachedValue;
    bool _dirty = true;

    public FloatStat(float baseValue)
    {
        BaseValue = baseValue;
    }

    public float Value
    {
        get
        {
            if (_dirty) Recalculate();
            return _cachedValue;
        }
    }

    private void Recalculate()
    {
        float val = BaseValue;
        foreach (var mod in _modifiers)
            val = mod.Apply(val);
        _cachedValue = val;
        _dirty = false;
    }

    public void AddModifier(FloatModifier mod)
    {
        _modifiers.Add(mod);
        _dirty = true;
    }

    public void RemoveModifier(FloatModifier mod)
    {
        _modifiers.Remove(mod);
        _dirty = true;
    }
}
