public readonly struct InventoryRuntimeDependencies
{
    public readonly WeaponInventoryDependencies WeaponInventoryDependencies;
    public readonly CurrencyInventoryDependencies CurrencyInventoryDependencies;
    public readonly PassiveItemInventoryDependencies PassiveItemInventoryDependencies;


    public InventoryRuntimeDependencies(WeaponInventoryDependencies weaponinvDep, CurrencyInventoryDependencies currencyDep, PassiveItemInventoryDependencies passiveDependencies)
    {
        WeaponInventoryDependencies = weaponinvDep;
        CurrencyInventoryDependencies = currencyDep;
        PassiveItemInventoryDependencies = passiveDependencies;
    }

}
