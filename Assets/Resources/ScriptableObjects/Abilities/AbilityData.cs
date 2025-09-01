using System.Collections.Generic;
using System;
using UnityEngine;
using SerializeReferenceEditor;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/Ability Data")]
public class AbilityData : ScriptableObject
{
    public Sprite icon;
    public AudioClip SFXClip;
    [SerializeReference, SR] public List<AbilityEffect> effects;
    public AbilityState state;
    public float energyCost;
    public string abilityName;
    public float strength;
    public float duration;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(abilityName)) abilityName = base.name;
        if (effects == null) effects = new List<AbilityEffect>();

        state = AbilityState.INACTIVE;
    }

    public bool CanBeUsed()
    {
        return state != AbilityState.ACTIVE;
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
        Rigidbody2D userRigidbody = context.user.GetComponent<Rigidbody2D>();    
        if (userRigidbody)
        {
            if (context.customData.TryGetValue("StateData", out var obj) && obj is UnitStateSO stateData)
            {
                if (context.user.TryGetComponent(out MonoBehaviour runner))
                {
                    runner.StartCoroutine(DisplacementEffectTimer(stateData, abilityData.duration));
                    userRigidbody.velocity = context.direction.normalized * abilityData.strength;
                }
            }
        }
    }
    
    IEnumerator DisplacementEffectTimer(UnitStateSO stateData, float duration)
    {
        stateData.canMove = false;
        yield return new WaitForSeconds(duration);
        stateData.canMove = true;
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
        if (context.customData.TryGetValue("UnitInfo", out var obj) && obj is UnitInfo unitInfo)
        {
            if (context.user.TryGetComponent(out MonoBehaviour runner))
            {
                runner.StartCoroutine(InvulnTimer(unitInfo, abilityData.duration));
            }

        }
    }

    IEnumerator InvulnTimer(UnitInfo unitInfo, float duration)
    {
        unitInfo.isInvuln = true;
        yield return new WaitForSeconds(duration);
        unitInfo.isInvuln = false;
    }

    public override void End(AbilityContext context)
    { }


}

public enum AbilityState
{
    INACTIVE,
    ACTIVE,
}



