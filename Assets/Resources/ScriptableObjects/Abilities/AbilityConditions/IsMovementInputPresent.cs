using System;
using UnityEngine;

[Serializable]
public class IsMovementInputPresent : AbilityConditions
{
    public override ConditionResult Evaluate(AbilityContext context)
    {
        if (Mathf.Approximately(PlayerInputManager.Instance.MovementInput.magnitude, 0) == true)
            return ConditionResult.Failure("The user is not inputting any movement.");

        return ConditionResult.Success();
    }

}
