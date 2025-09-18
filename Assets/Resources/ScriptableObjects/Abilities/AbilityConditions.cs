using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[Serializable]
public abstract class AbilityConditions
{
    public ConditionResult conditionResult;
    public string conditionName;
    public string conditionDescription; 
    public abstract ConditionResult Evaluate(AbilityContext context);
}

[Serializable]
public class IsMoving : AbilityConditions
{
    public override ConditionResult Evaluate(AbilityContext context)
    {
        if (context.user.GetComponent<Rigidbody2D>().velocity.magnitude == 0)
            return ConditionResult.Failure("The user is not moving.");

        return ConditionResult.Success();
    }
}


[Serializable]
public class IsMovementInputPresent : AbilityConditions
{
    public override ConditionResult Evaluate(AbilityContext context)
    {
        if (Mathf.Approximately(PlayerInputManager.instance.MovementInput.magnitude, 0) == true)
            return ConditionResult.Failure("The user is not inputting any movement.");

        return ConditionResult.Success();
    }

}

public struct ConditionResult
{
    public bool isSucessful;
    public string reason;

    public static ConditionResult Success() => new() { isSucessful = true };
    public static ConditionResult Failure(string reason) => new() { isSucessful = false, reason = reason };
}