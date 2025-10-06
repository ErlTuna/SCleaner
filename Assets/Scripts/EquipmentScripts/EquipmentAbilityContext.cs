using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAbilityContext
{
    public GameObject User;
    public EquipmentData EquipmentRuntimeData;
    public List<GameObject> AffectedTargets = new();
    public Dictionary<string, object> customData = new();

    public EquipmentAbilityContext(GameObject user, EquipmentData equipmentData, List<GameObject> affectedTargets)
    {
        User = user;
        EquipmentRuntimeData = equipmentData;
        AffectedTargets = affectedTargets;
    }

}
