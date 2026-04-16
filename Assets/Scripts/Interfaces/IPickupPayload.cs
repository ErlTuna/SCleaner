using System;

public interface IPickupPayload { }

// Wraps runtime data of a weapon for a weapon payload.
public sealed class WeaponPickupPayload : IPickupPayload
{
    public WeaponRuntime RuntimeData { get; }

    public WeaponPickupPayload(WeaponRuntime runtimeData)
    {
        RuntimeData = runtimeData;
    }
}

public sealed class PassiveItemPickupPayload : IPickupPayload
{
    public PassiveItemSO PassiveItemSO {get; }

    public PassiveItemPickupPayload(PassiveItemSO passiveItem)
    {
        PassiveItemSO = passiveItem;
    }
}

public sealed class CurrencyPickupPayload : IPickupPayload
{
    public int Amount { get; }

    public CurrencyPickupPayload(int amount)
    {
        Amount = amount; 
    }

}

