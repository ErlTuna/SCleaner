using System.Collections.Generic;
using UnityEngine;

public class AbilityContext
{
    public Unit user;
    public UnitRuntimeDataHolder userRuntimeData;
    public Unit target;
    public UnitRuntimeDataHolder targetRuntimeData;
    public Vector2 direction;
    public Dictionary<string, object> customData = new();
}
