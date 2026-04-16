using System;

[Serializable]
public class UnitMovementData
{
    public FloatStat CurrentMovementSpeed {get; set; }

    public UnitMovementData(UnitMovementConfigSO movementConfig)
    {
        //CurrentMovementSpeed = movementConfig.maxMovementSpeed;
        CurrentMovementSpeed = new FloatStat(movementConfig.StartingMovementSpeed);
    }
}
