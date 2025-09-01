using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit SOs", menuName = "ScriptableObjects/Component SOs/Unit State")]
public class UnitStateSO : ScriptableObject
{
    public bool isAlive = true;
    public bool isInvuln = false;
    public bool canMove = false;
}


