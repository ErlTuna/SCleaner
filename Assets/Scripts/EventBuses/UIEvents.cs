using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action<Sprite> OnAbilityChanged;

    public static void RaiseAbilityChanged(Sprite abilityIcon)
    {
        OnAbilityChanged?.Invoke(abilityIcon);
    }
}
