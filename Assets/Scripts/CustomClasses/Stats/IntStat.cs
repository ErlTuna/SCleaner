using System;
using System.Collections.Generic;

[Serializable]
public class IntStat
{
    public int BaseValue {get; private set;}
    readonly List<IntModifier> _modifiers = new();
    int _cachedValue;
    bool _dirty = true;

    public IntStat(int baseValue)
    {
        BaseValue = baseValue;
    }

    public int Value
    {
        get
        {
            if (_dirty) Recalculate();
            return _cachedValue;
        }
    }

    private void Recalculate()
    {
        int val = BaseValue;
        foreach (var mod in _modifiers)
            val = mod.Apply(val);
        _cachedValue = val;
        _dirty = false;
    }

    public void AddModifier(IntModifier mod)
    {
        _modifiers.Add(mod);
        _dirty = true;
    }

    public void RemoveModifier(IntModifier mod)
    {
        _modifiers.Remove(mod);
        _dirty = true;
    }
}


