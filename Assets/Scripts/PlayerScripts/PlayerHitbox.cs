using System;
using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    [SerializeField] CircleCollider2D _hitbox;
    [SerializeField] UnitStateData _playerStateData;
    bool _previousInvulnState;

    public void InitializeStateData(UnitStateData playerStateData)
    {
        _playerStateData = playerStateData;
    }

    void Update()
    {
        bool isInvuln = _playerStateData.IsHitInvuln || _playerStateData.IsInvuln;

        if (isInvuln != _previousInvulnState)
        {
            _hitbox.enabled = !isInvuln;
            _previousInvulnState = isInvuln;
        }
    }
    
}
