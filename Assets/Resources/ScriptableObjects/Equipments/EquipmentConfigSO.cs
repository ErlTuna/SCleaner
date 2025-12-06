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
    public Sprite UI_Icon;
    public EquipmentAbilityConfig Ability;
    public int StartingCarge;
    public int MaxCharge;
    public float MaxLifetime;
}
