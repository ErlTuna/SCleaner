using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player SOs", menuName = "ScriptableObjects/Player/Player Stats")]
public class UnitStatsSO : ScriptableObject
{
    public float movementSpeed;
    public float maxMovementSpeed = 5f;
    
}
