using System.Collections.Generic;
using System;
using UnityEngine;
using SerializeReferenceEditor;
using System.Collections;

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
    public Sprite Icon;
    public Sprite UI_Icon;
    public AudioClip SFXClip;

    [Header("Effects and Conditions")]
    [SerializeReference, SR] public List<AbilityEffect> Effects;
    [SerializeReference, SR] public List<AbilityConditions> Conditions;



    void OnEnable()
    {
        if (string.IsNullOrEmpty(AbilityName)) AbilityName = name;
        Effects ??= new List<AbilityEffect>();

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
            if (result.IsSucessful != true)
            {
                Debug.Log(result.Reason);
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
public enum AbilityState
{
    INACTIVE,
    ACTIVE,
}





