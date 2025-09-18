using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRuntimeDataHolder : UnitRuntimeDataHolder
{
    public PlayerRuntimeDataHolder()
    {
        AddRuntimeData(new PlayerHealthData());
        AddRuntimeData(new PlayerMovementData());
        AddRuntimeData(new PlayerStateData());
        AddRuntimeData(new PlayerInventoryData());
        AddRuntimeData(new PlayerEnergyData());
    }
}










    
        

        

    
