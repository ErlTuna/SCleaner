using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action<AbilityData> OnAbilityUsed;
    public static event Action<Sprite> OnAbilityChanged;
    public static event Action<float, float> OnEnergyChanged;
    public static event Action<float, float> OnEnergyRecovered;
    public static event Action OnEnergyUsed;

    public static void RaiseEnergyChanged(float currentEnergy, float maxEnergy)
    {
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    public static void RaiseEnergyUsed()
    {
        OnEnergyUsed?.Invoke();
    }

    public static void RaiseEnergyRecovered(float currentEnergy, float maxEnergy)
    {
        OnEnergyRecovered?.Invoke(currentEnergy, maxEnergy);
    }

    public static void RaiseAbilityUsed(AbilityData data)
    {
        OnAbilityUsed?.Invoke(data);
    }

    public static void RaiseAbilityChanged(Sprite abilityIcon)
    {
        OnAbilityChanged?.Invoke(abilityIcon);
    }
}
