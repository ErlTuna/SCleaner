using System;
using UnityEngine;

[Serializable]
public class EnemyStateData : UnitStateData
{
    public bool PlayerWithinAttackRange = false;
    public bool HasBeenAttacked = false;
    public bool HasAttacked = false;
    public bool IsRecoveringPostAttack = false;
    public bool IsRoaming = false;
    public bool HasDetectedPlayer = false;
    public bool HasLineOfSight = false;
    public bool IsChargingAnAttack = false;
    public bool IsAttacking = false;
    public bool IsRecovering = false;

    public EnemyStateData(UnitStateConfigSO config) : base(config)
    {
        if (config == null)
        {
            Debug.Log("StateData config missing!");
            return;
        }

        if (config is EnemyStateConfigSO enemyConfig)
        {
            PlayerWithinAttackRange = enemyConfig.PlayerWithinAttackRange;
            HasBeenAttacked = false;
            HasAttacked = false;
            IsRecoveringPostAttack = false;
            IsRoaming = false;
            HasDetectedPlayer = false;
            IsChargingAnAttack = false;
            IsAttacking = false;
            IsRecovering = false;
        }

    }

}
