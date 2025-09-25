using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitStateData : IUnitStateData
{
    public bool IsAlive { get; set; }
    public bool IsHitInvuln { get; set;}
    public bool IsInvuln { get; set; }
    public bool CanMove { get; set; }
    public bool IsShielded { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO configWrapper)
    {
        ConfigureWith(configWrapper.StateConfig);
    }

    public void ConfigureWith(UnitStateConfigSO config)
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
