using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health X", menuName = "ScriptableObjects/Component SOs/Movement")]
public class UnitMovementConfigSO : ScriptableObject
{
    public float movementSpeed;
    public float maxMovementSpeed = 5f;
}
