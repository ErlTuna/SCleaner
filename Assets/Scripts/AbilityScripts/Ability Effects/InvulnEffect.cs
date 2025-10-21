using System;
using System.Collections;
using UnityEngine;

[Serializable]
class InvulnEffect : AbilityEffect
{
    public float Duration = 1f;
    public override void Execute(AbilityContext context, AbilityData abilityData)
    {
        Duration = abilityData.Duration;

        if (context.userRuntimeData == null)
        {
            Debug.Log("User has no runtime data");
            return;
        }

        if (context.user.TryGetComponent(out MonoBehaviour runner) == false)
        {
            Debug.Log("User has no monobehaviour");
            return;
        }

        if (context.userRuntimeData.TryGetRuntimeData(out IUnitStateData stateData))
        {
            runner.StartCoroutine(InvulnTimer(stateData, Duration));
        }


    }

    IEnumerator InvulnTimer(IUnitStateData userState, float duration)
    {
        userState.IsInvuln = true;
        yield return new WaitForSeconds(duration);
        userState.IsInvuln = false;
    }

    public override void End(AbilityContext context)
    { }
}
