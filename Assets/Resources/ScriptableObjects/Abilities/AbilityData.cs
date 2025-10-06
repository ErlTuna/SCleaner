using System.Collections.Generic;
using System;
using UnityEngine;
using SerializeReferenceEditor;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/Ability/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("General Information")]
    public string AbilityName;
    public GameObject Owner;
    public AbilityType AbilityType;
    public AbilityState State;
    public float EnergyCost;
    public float Duration;

    [Header("Visual & SFX")]
    public Sprite icon;
    public AudioClip SFXClip;

    [Header("Effects and Conditions")]
    [SerializeReference, SR] public List<AbilityEffect> Effects;
    [SerializeReference, SR] public List<AbilityConditions> Conditions;



    void OnEnable()
    {
        if (string.IsNullOrEmpty(AbilityName)) AbilityName = name;
        if (Effects == null) Effects = new List<AbilityEffect>();

        State = AbilityState.INACTIVE;
    }

    public bool CanBeUsed(AbilityContext context)
    {
        if (State != AbilityState.INACTIVE)
        {
            Debug.Log("Ability is already active!");
            return false;
        }


        foreach (AbilityConditions condition in Conditions)
        {
            ConditionResult result = condition.Evaluate(context);
            if (result.isSucessful != true)
            {
                Debug.Log(result.reason);
                return false;
            }

        }

        return true;
    }


    public IEnumerator AbilityTriggered(Action abilityFinishedCallback)
    {
        State = AbilityState.ACTIVE;

        yield return new WaitForSeconds(Duration);

        State = AbilityState.INACTIVE;
        abilityFinishedCallback?.Invoke();
    }
}

public enum AbilityType
{
    INSTANTENOUS,
    OVERTIME
}


[Serializable]
public abstract class AbilityEffect
{
    public abstract void Execute(AbilityContext context, AbilityData abilityData);
    public abstract void End(AbilityContext context);
}

[Serializable]
class DisplacementEffect : AbilityEffect
{
    public float Strength = 1f;
    public float Duration = 1f;
    public IUnitStateData userStateData;
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

        Rigidbody2D userRigidbody = context.user.GetComponent<Rigidbody2D>();
        if (userRigidbody == null)
        {
            Debug.Log("User has no rigidbody");
            return;
        }

        if(abilityData.SFXClip)
            AudioSource.PlayClipAtPoint(abilityData.SFXClip, context.user.transform.position);

        runner.StartCoroutine(DisplacementEffectTimer(context.userRuntimeData, Duration));
        userRigidbody.velocity = context.direction.normalized * Strength;

    }
    
    IEnumerator DisplacementEffectTimer(UnitRuntimeDataHolder unitRuntimeDataHolder, float duration)
    {
        if (unitRuntimeDataHolder.TryGetRuntimeData(out IUnitStateData userStateData))
        {
            if (!userStateData.CanMove)
            {
                Debug.LogWarning("Unit is already unable to move?..");
                yield break;
            }
            userStateData.CanMove = false;
            yield return new WaitForSeconds(duration);
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

[Serializable]



public enum AbilityState
{
    INACTIVE,
    ACTIVE,
}





