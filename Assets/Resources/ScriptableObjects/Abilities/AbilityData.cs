using System.Collections.Generic;
using System;
using UnityEngine;
using SerializeReferenceEditor;
using System.Collections;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/Ability/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("General Information")]
    public string abilityName;
    public Unit _owner;
    public AbilityType abilityType;
    public AbilityState state;
    public float energyCost; 
    public float strength;
    public float duration;

    [Header("Visual & SFX")]
    public Sprite icon;
    public AudioClip SFXClip;

    [Header("Effects and Conditions")]
    [SerializeReference, SR] public List<AbilityEffect> effects;
    [SerializeReference, SR] public List<AbilityConditions> conditions;



    void OnEnable()
    {
        if (string.IsNullOrEmpty(abilityName)) abilityName = name;
        if (effects == null) effects = new List<AbilityEffect>();

        state = AbilityState.INACTIVE;
    }

    public bool CanBeUsed(AbilityContext context)
    {
        if (state != AbilityState.INACTIVE)
        {
            Debug.Log("Ability is already active!");
            return false;
        }
            

        foreach (AbilityConditions condition in conditions)
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
        state = AbilityState.ACTIVE;

        yield return new WaitForSeconds(duration);

        state = AbilityState.INACTIVE;
        abilityFinishedCallback?.Invoke();
    }
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
    public override void Execute(AbilityContext context, AbilityData abilityData)
    {
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
                
        runner.StartCoroutine(DisplacementEffectTimer(context.userRuntimeData, abilityData.duration));
        userRigidbody.velocity = context.direction.normalized * abilityData.strength;
                
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

    }

}

[Serializable]
class InvulnEffect : AbilityEffect
{
    public override void Execute(AbilityContext context, AbilityData abilityData)
    {

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
            runner.StartCoroutine(InvulnTimer(stateData, abilityData.duration));
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

class ShieldEffect : AbilityEffect
{
    public override void Execute(AbilityContext context, AbilityData abilityData)
    {
        
    }



    public override void End(AbilityContext context)
    {
        //
    }

    
}

public enum AbilityState
{
    INACTIVE,
    ACTIVE,
}



