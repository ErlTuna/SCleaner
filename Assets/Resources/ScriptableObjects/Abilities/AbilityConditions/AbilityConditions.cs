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


public struct ConditionResult
{
    public bool IsSucessful;
    public string Reason;

    public static ConditionResult Success() => new() { IsSucessful = true };
    public static ConditionResult Failure(string reason) => new() { IsSucessful = false, Reason = reason };
}