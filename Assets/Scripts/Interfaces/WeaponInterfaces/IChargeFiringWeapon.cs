interface IChargeFiringWeapon : IWeaponAttackInputHandler
{
    bool CanCharge();
    void BeginCharge();
    void CancelCharge();
    void PerformAttack();
}
