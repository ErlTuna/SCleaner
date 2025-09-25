using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Config", menuName = "ScriptableObjects/Component Configs/Movement Config")]
public class UnitMovementConfigSO : ScriptableObject
{
    public float movementSpeed;
    public float maxMovementSpeed = 5f;
}
