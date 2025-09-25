using System.Collections;
using System.Collections.Generic;
using System.IO;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Config", menuName = "ScriptableObjects/Equipment Config")]
public class EquipmentConfigSO : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
    public EquipmentAbilityData Ability;
    public EquipmentType EquipmentType;
    public int StartingCount;
    public int MaxCount;
    public float MaxLifetime;
}
