using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CurrencyCounter : MonoBehaviour
{
    [SerializeField] IntEventChannelSO _currencyPickedUpEventChannel;
    [SerializeField] TextMeshProUGUI _currencyCounter;
    int currentCounterValue;


    void OnEnable()
    {
        _currencyPickedUpEventChannel.OnEventRaised += UpdateCounter;
    }

    void OnDisable()
    {
        _currencyPickedUpEventChannel.OnEventRaised -= UpdateCounter;
    }

    void Awake()
    {
        _currencyCounter.SetText("0");
    }

    void UpdateCounter(int value)
    {
        _currencyCounter.SetText(value.ToString());
    }

}
