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

    void UpdateAbilityDisplay(Sprite abilityIcon)
    {
        if (abilityIcon != null)
        {
            _icon.color = Color.white;
            _icon.sprite = abilityIcon;
        }
            
    }
}
