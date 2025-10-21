using System;
using System.Collections;
using UnityEngine;

[Serializable]
class DisplacementEffect : AbilityEffect
{
    public float Strength = 1f;
    public float Duration = 1f;
    public bool EmitAfterImage = true;
    public IUnitStateData userStateData;
    AbilityContext _context;

    public override void Execute(AbilityContext context, AbilityData abilityData)
    {
        Duration = abilityData.Duration;
        _context = context;

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

        if (context.user.TryGetComponent<Rigidbody2D>(out var userRigidbody) == false)
        {
            Debug.Log("User has no rigidbody");
            return;
        }

        if (abilityData.SFXClip)
            AudioSource.PlayClipAtPoint(abilityData.SFXClip, context.user.transform.position);

        runner.StartCoroutine(DisplacementEffectTimer(context.userRuntimeData, Duration));
        userRigidbody.velocity = context.direction.normalized * Strength;

    }

    IEnumerator DisplacementEffectTimer(UnitRuntimeDataHolder unitRuntimeDataHolder, float duration)
    {
        float elapsedTime = 0f;
        if (unitRuntimeDataHolder.TryGetRuntimeData(out IUnitStateData userStateData))
        {
            if (!userStateData.CanMove)
            {
                Debug.LogWarning("Unit is already unable to move?..");
                yield break;
            }

            userStateData.CanMove = false;
            AfterImageEmitter emitter = _context.user.GetComponentInChildren<AfterImageEmitter>();

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

        else
            Debug.LogError("DisplacementEffectTimer failed: IUnitStateData not found.");
    }

    public override void End(AbilityContext context)
    {
        userStateData.CanMove = true;
    }

}
