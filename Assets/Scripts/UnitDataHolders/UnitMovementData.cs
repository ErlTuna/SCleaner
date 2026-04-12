using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitMovementData
{
    //public float CurrentMovementSpeed { get; set; }
    public FloatStat CurrentMovementSped {get; set; }

    public UnitMovementData(UnitMovementConfigSO movementConfig)
    {
        //CurrentMovementSpeed = movementConfig.maxMovementSpeed;
        CurrentMovementSped = new FloatStat(movementConfig.maxMovementSpeed);
    }
}
