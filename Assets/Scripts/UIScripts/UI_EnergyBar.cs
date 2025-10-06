using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnergyBar : MonoBehaviour
{
    [SerializeField] Image _energyBar;
    [SerializeField] Image _fill;

    void OnEnable()
    {
        UIEvents.OnEnergyChanged += UpdateEnergyBar;
    }

    void OnDisable()
    {
        UIEvents.OnEnergyChanged -= UpdateEnergyBar;
    }

    void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        _fill.fillAmount = currentEnergy / maxEnergy;
    }
}
