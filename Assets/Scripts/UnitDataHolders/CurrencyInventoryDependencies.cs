public readonly struct CurrencyInventoryDependencies
{
    public readonly IntEventChannelSO CurrencyPickedUpEventChannel;

    public CurrencyInventoryDependencies(IntEventChannelSO eventChannel)
    {
        CurrencyPickedUpEventChannel = eventChannel;
    }
}
