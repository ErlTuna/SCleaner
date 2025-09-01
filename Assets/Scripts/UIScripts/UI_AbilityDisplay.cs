using UnityEngine;
using UnityEngine.UI;

public class UI_AbilityDisplay : MonoBehaviour
{
    [SerializeField] Image _icon;

    void OnEnable()
    {
        UIEvents.OnAbilityChanged += UpdateAbilityDisplay;
    }

    void OnDisable()
    {
        UIEvents.OnAbilityChanged -= UpdateAbilityDisplay;
    }

    void UpdateAbilityDisplay(AbilityData abilityData)
    {
        if (abilityData != null) _icon.sprite = abilityData.icon;
    }
}
