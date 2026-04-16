using System;
using UnityEngine;

[Serializable]
public class UnitStateData
{
    public bool IsAlive;
    public bool IsHitInvuln;
    public bool IsInvuln;
    public bool CanMove;
    public bool IsShielded;

    public UnitStateData(UnitStateConfigSO config)
    {
        if (config == null)
        {
            Debug.Log("StateData config missing!");
            return;
        }

        IsAlive = config.isAlive;
        IsInvuln = config.isInvuln;
        CanMove = config.canMove;
        IsShielded = config.isShielded;

    }
}
