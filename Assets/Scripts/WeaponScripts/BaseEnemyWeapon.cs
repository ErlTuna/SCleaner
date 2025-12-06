using UnityEngine;

public abstract class BaseEnemyWeapon : BaseWeapon
{
    [SerializeField] protected AttackPatternSO attackPattern;
    public AttackPatternSO AttackPattern => attackPattern;
    public abstract void ExecuteAttackPattern();
    public void PrepAttackPattern()
    {
        attackPattern = Instantiate(AttackPattern);
    }

}
