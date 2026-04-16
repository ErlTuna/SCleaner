public readonly struct PassiveItemInventoryDependencies
{
    
    public readonly ItemPickedUpEventChannelSO PickedUpChannel;
    public readonly ItemDroppedEventChannel DroppedChannel;

    public PassiveItemInventoryDependencies(ItemPickedUpEventChannelSO itemPickedUpChannel, ItemDroppedEventChannel itemDroppedEventChannel)
    {
        PickedUpChannel = itemPickedUpChannel;
        DroppedChannel = itemDroppedEventChannel;
    }
}
