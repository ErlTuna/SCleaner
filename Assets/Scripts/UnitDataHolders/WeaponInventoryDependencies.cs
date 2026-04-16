public readonly struct WeaponInventoryDependencies
{
    public readonly WeaponDatabaseSO Database;
    public readonly ItemPickedUpEventChannelSO AddedChannel;
    public readonly ItemDroppedEventChannel DroppedChannel;

    public WeaponInventoryDependencies(WeaponDatabaseSO database, ItemPickedUpEventChannelSO added, ItemDroppedEventChannel dropped)
    {
        Database = database;
        AddedChannel = added;
        DroppedChannel = dropped;
    }
}
