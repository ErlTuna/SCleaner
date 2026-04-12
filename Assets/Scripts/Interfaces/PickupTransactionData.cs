public sealed class PickupTransactionData
{
    public ItemSO PickupData { get; }
    public IPickupPayload Payload { get; }

    public PickupTransactionData(ItemSO pickupData, IPickupPayload payload = null)
    {
        PickupData = pickupData;
        Payload = payload;
    }

    public bool TryGetPayload<T>(out T payload) where T : class, IPickupPayload
    {
        payload = Payload as T;
        return payload != null;
    }
}


