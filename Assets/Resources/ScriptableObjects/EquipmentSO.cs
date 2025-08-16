using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/Equipment Info")]
public class EquipmentSO : ScriptableObject
{
    public String itemName;
    public Sprite sprite;
    public AudioClip activatedSFX;
    public AudioClip explosionAudio;
}
