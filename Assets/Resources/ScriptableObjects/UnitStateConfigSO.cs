using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State Config", menuName = "ScriptableObjects/Component Configs/Unit State Config")]
public class UnitStateConfigSO : ScriptableObject
{
    public bool isAlive = true;
    public bool isInvuln = false;
    public bool canMove = false;
    public bool isShielded = false;
}

