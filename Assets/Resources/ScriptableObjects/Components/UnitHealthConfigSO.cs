using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Config", menuName = "ScriptableObjects/Component Configs/Health Config")]
public class UnitHealthConfigSO : ScriptableObject
{
    public int Health;
    public int MaxHealth;
    public int StartingShieldHealth;
    public AudioClip OnHitSFX;
}
