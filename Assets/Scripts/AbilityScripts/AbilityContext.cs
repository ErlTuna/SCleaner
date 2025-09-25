using System.Collections.Generic;
using UnityEngine;

public class AbilityContext
{
    public GameObject user;
    public UnitRuntimeDataHolder userRuntimeData;
    public List<GameObject> targets;
    public UnitRuntimeDataHolder targetRuntimeData;
    public Vector2 direction;
    public Dictionary<string, object> customData = new();
}
