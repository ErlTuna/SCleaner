using System;
using UnityEngine;

public class UnitCurrencyInventory
{
    public int OwnedCurrency;
    public int MaxCurrency;
    readonly IntEventChannelSO _currencyPickedUpEventChannel;

    public UnitCurrencyInventory(int ownedCurrency, int maxCurrency, IntEventChannelSO currencyPickedUpEventChannel)
    {
        OwnedCurrency = ownedCurrency;
        MaxCurrency = maxCurrency;
        _currencyPickedUpEventChannel = currencyPickedUpEventChannel;
    }

    public void AddCurrency(int amount)
    {
        //Debug.Log("Picked up : " + amount + " currency.");
        OwnedCurrency = Mathf.Min(OwnedCurrency + amount, MaxCurrency);
        _currencyPickedUpEventChannel.RaiseEvent(OwnedCurrency);
    }
}
