using System;
using UnityEngine;

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
