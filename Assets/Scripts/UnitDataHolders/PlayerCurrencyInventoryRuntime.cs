using UnityEngine;

public class PlayerCurrencyInventoryRuntime
{   
    readonly PlayerCurrencyInventoryData _currencyData;
    readonly IntEventChannelSO _currencyPickedUpEventChannel;

    public PlayerCurrencyInventoryRuntime(PlayerCurrencyInventoryData currencyData, CurrencyInventoryDependencies dependencies)
    {
        _currencyData = currencyData;
        _currencyPickedUpEventChannel = dependencies.CurrencyPickedUpEventChannel;
    }

    public void AddCurrency(int amount)
    {
        //Debug.Log("Picked up : " + amount + " currency.");
        _currencyData.Current = Mathf.Min(_currencyData.Current + amount, _currencyData.Max);
        _currencyPickedUpEventChannel.RaiseEvent(_currencyData.Current);
    }
}
