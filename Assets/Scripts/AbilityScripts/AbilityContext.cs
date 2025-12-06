using System.Collections.Generic;
using UnityEngine;

public class AbilityContext
{
    public readonly GameObject user;
    public readonly UnitStateData userStateData;
    public readonly List<GameObject> targets;
    public readonly Vector2 direction;
    
    //public readonly Dictionary<string, object> customData = new();

    public AbilityContext(GameObject user, List<GameObject> targets, Vector2 direction, UnitStateData userStateData)
    {
        this.user = user;
        this.targets = targets;
        this.direction = direction;
        this.userStateData = userStateData;
    }
}
