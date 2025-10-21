using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyWeapon : BaseWeapon
{
    [SerializeField] protected AttackPatternSO attackPattern;
    public AttackPatternSO AttackPattern => attackPattern;
    public abstract void ExecuteAttackPattern();

}
