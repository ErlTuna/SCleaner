using System;
using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentAbilityConfig", menuName = "ScriptableObjects/Equipment/Equipment Ability Config")]
public class EquipmentAbilityConfig : ScriptableObject
{
    [Header("General Information")]
    public string AbilityName;
    public GameObject Owner;

    [Header("Visual & SFX")]
    public Sprite icon;
    public AudioClip SFXClip;

    [Header("Effects and Conditions")]
    [SerializeReference, SR] public List<EquipmentEffect> Effects;
    [SerializeReference, SR] public List<EquipmentEffect> ActivationEffects = new();
    [SerializeReference, SR] public List<EquipmentEffect> OverTimeEffects = new();
    [SerializeReference, SR] public List<EquipmentEffect> ExpirationEffects = new();
    [SerializeReference, SR] public List<AbilityConditions> Conditions;
}
