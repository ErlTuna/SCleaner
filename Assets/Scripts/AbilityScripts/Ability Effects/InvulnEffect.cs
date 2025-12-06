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

        if (context.userStateData == null)
        {
            Debug.Log("User has no state data");
            return;
        }
        

        if (context.user.TryGetComponent(out MonoBehaviour runner) == false)
        {
            Debug.Log("User has no monobehaviour");
            return;
        }


        runner.StartCoroutine(InvulnTimer(context.userStateData, Duration));
        
    }

    IEnumerator InvulnTimer(UnitStateData userStateData, float duration)
    {
        userStateData.IsInvuln = true;
        yield return new WaitForSeconds(duration);
        userStateData.IsInvuln = false;
    }

    public override void End(AbilityContext context){ }
}
