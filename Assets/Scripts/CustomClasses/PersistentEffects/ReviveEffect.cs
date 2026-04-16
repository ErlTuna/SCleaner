using System;
using UnityEngine;

public class ReviveEffect : IPersistentEffect, IDefeatInterceptor
{
    int _healAmount = 0;
    bool _isConsumed = false;
    bool _singleUse = true;

    public event Action<IPersistentEffect> OnConsumed;
    IDefeatable _defeatable;
    IHealable _healable;

    public ReviveEffect(int healAmount, bool singleUse)
    {
        _healAmount = healAmount;
        _singleUse = singleUse;
    } 

    public void Attach(EffectContext ctx, Action<IPersistentEffect> onConsumed)
    {
        ctx.TryGet(out _defeatable);
        ctx.TryGet(out _healable);
        if (_healable == null)
        {
            Debug.Log("Healable is null.");
        }
        if (_defeatable == null)
        {
            Debug.Log("defeatable is null");
            return;
        }
        
        _defeatable.OnBeforeDefeat += OnBeforeDefeat;
        OnConsumed = onConsumed;

        Debug.Log("REVIVE EFFECT WAS ADDED TO PLAYER.");
    }

    public void Detach()
    {
        _defeatable.OnBeforeDefeat -= OnBeforeDefeat;
        OnConsumed = null;
    }

    private bool OnBeforeDefeat()
    {
        if (_isConsumed) return false;

        Debug.Log("Attempting to heal...");
        _healable.Heal(_healAmount);

        if (_singleUse)
        {
            _isConsumed = true;
            OnConsumed.Invoke(this);
        }

        return true;
    }
}
