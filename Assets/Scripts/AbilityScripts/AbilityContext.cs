using System.Collections.Generic;
using UnityEngine;

public class AbilityContext
{
    public GameObject user;
    public GameObject target;
    public Vector2 direction;
    public Dictionary<string, object> customData = new();
}
