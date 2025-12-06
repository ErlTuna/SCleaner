using System;
using UnityEngine;


[Serializable]
public abstract class AbilityConditions
{
    public ConditionResult conditionResult;
    public string conditionName;
    public string conditionDescription; 
    public abstract ConditionResult Evaluate(AbilityContext context);
}
