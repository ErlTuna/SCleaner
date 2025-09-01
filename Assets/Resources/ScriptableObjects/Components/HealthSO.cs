using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health X", menuName = "ScriptableObjects/Component SOs/Health")]
public class HealthSO : ScriptableObject
{
    public int health;
    public int maxHealth = 20;
}
