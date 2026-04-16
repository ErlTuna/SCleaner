using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EffectContext
{
    readonly IReadOnlyDictionary<Type, IManagerComponent> _managers;

    public EffectContext(IReadOnlyDictionary<Type, IManagerComponent> managers)
    {
        // Cache references to all relevant managers
        _managers = managers;
    }

    public bool TryGet<T>(out T manager) where T : class
    {
        if (_managers.TryGetValue(typeof(T), out var found))
        {
            manager = found as T;
            return manager != null;
        }

        manager = null;
        Debug.LogWarning($"Manager of type {typeof(T)} not found or cast failed!");
        return false;
    }
}
