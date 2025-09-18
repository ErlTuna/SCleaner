using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Config", menuName = "ScriptableObjects/Component Configs/Health Config")]
public class UnitHealthConfigSO : ScriptableObject
{
    public int health;
    public int maxHealth = 20;
}
