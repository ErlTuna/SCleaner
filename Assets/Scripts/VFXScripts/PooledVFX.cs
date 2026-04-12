using System;
using UnityEngine;

public class PooledVFX : MonoBehaviour
{   
    Action _onFinished;
    Action<PooledVFX> _returnToPoolAction;
    Action<PooledVFX> _overrideAction;
    [SerializeField] Animator _animator;

    // Can supply an override.
    public void Play(Action onFinished = null)
    {
        _onFinished = onFinished;
        _animator.Play("SpawnVFX", 0, 0f);
    }

    public void SetReturnAction(Action<PooledVFX> returnAction)
    {
        _returnToPoolAction = returnAction;
    }


    // Called by Animation Event.
    public void OnAnimationCompleted()
    {
        _onFinished?.Invoke();
        
        if (_overrideAction != null)
        {
            _overrideAction(this);
            _overrideAction = null;
        }
        else
            _returnToPoolAction?.Invoke(this);

        
        
    }
}
