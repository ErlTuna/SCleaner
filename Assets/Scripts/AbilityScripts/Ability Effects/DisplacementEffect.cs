using System;
using System.Collections;
using UnityEngine;

[Serializable]
class DisplacementEffect : AbilityEffect
{
    public float Strength = 1f;
    public float Duration = 1f;
    public bool EmitAfterImage = true;
    GameObject _user;

    public override void Execute(AbilityContext context, AbilityData abilityData)
    {
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

        if (context.user.TryGetComponent<Rigidbody2D>(out var userRigidbody) == false)
        {
            Debug.Log("User has no rigidbody");
            return;
        }

        Duration = abilityData.Duration;
        _user = context.user;

        if (abilityData.SFXClip)
        {
            AudioSource.PlayClipAtPoint(abilityData.SFXClip, context.user.transform.position);
        }
            

        runner.StartCoroutine(DisplacementEffectTimer(context.userStateData, Duration));
        userRigidbody.velocity = context.direction.normalized * Strength;

    }

    IEnumerator DisplacementEffectTimer(UnitStateData userStateData, float duration)
    {
        float elapsedTime = 0f;
        if (!userStateData.CanMove)
        {
            Debug.LogWarning("Unit is already unable to move?..");
            yield break;
        }

        userStateData.CanMove = false;
        AfterImageEmitter emitter = _user.GetComponentInChildren<AfterImageEmitter>();

        while (elapsedTime < duration)
        {
            if (EmitAfterImage)
                if (emitter != null)
                    emitter.TryEmit();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        userStateData.CanMove = true;

    }

    public override void End(AbilityContext context)
    {
        //userStateData.CanMove = true;
    }

}
