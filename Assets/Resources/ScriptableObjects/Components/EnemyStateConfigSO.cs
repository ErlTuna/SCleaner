using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State Config", menuName = "ScriptableObjects/Component Configs/Enemy State Config")]
public class EnemyStateConfigSO : UnitStateConfigSO
{
    public bool PlayerWithinAttackRange = false;
    public bool HasBeenAttacked = false;
    public bool HasAttacked = false;
    public bool IsRecoveringPostAttack = false;
    public bool IsRoaming = false;
    public bool HasDetectedPlayer = false;
    public bool IsChargingAnAttack = false;
    public bool IsAttacking = false;
    public bool IsRecovering = false;
}
