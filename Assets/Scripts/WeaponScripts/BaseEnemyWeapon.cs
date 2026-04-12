using UnityEngine;

public abstract class BaseEnemyWeapon : BaseWeapon<EnemyWeaponConfigSO>
{
    //[SerializeField] protected AttackPatternSO attackPattern;
    [SerializeField] protected Transform stockGripPoint;
    [SerializeField] protected Transform secondGripPoint;

    //public AttackPatternSO AttackPattern => attackPattern;
    public Transform StockGripPoint => stockGripPoint;
    public Transform SecondGripPoint => secondGripPoint;
    //public abstract void ExecuteAttackPattern();
    public void PrepAttackPattern()
    {
        //attackPattern = Instantiate(AttackPattern);
    }

}
