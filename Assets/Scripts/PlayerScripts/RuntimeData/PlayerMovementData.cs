using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerMovementData : IUnitMovementData
{
    public float CurrentMovementSpeed { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO configWrapper)
    {
        ConfigureWith(configWrapper.MovementConfig);
    }

    public void ConfigureWith(UnitMovementConfigSO config)
    {
        CurrentMovementSpeed = config.maxMovementSpeed;
    }
}
