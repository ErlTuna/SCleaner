using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateData : UnitStateData
{
    public EnemyCoreState CoreState = EnemyCoreState.Idle;

    // flags
    public bool PlayerWithinAttackRange = false;
    public bool HasBeenAttacked = false;
    public bool HasAttacked = false;
    public bool IsRecoveringPostAttack = false;

    public bool IsRoaming => CoreState == EnemyCoreState.Roaming;
    public bool HasDetectedPlayer => CoreState == EnemyCoreState.DetectedPlayer;
    public bool IsChargingAnAttack => CoreState == EnemyCoreState.ChargingAttack;
    public bool IsAttacking => CoreState == EnemyCoreState.Attacking;
    public bool IsRecovering => CoreState == EnemyCoreState.Recovering;

}


// The enemy can only be in one of these states at a time
// Example : Enemy can't be Idle AND Attacking at the same time.
public enum EnemyCoreState
{
    Roaming,
    Idle,
    DetectedPlayer,
    ChargingAttack,
    Attacking,
    Recovering,
    Stunned,
    Dead
}
