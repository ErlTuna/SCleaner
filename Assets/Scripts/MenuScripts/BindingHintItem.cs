using TMPro;
using UnityEngine;
using UnityEngine.UI;

// GameObject representation of a key bind. 
// "How are bindings represented in game?"
// With an icon and label.
public class BindingHintItem : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup _parentHLG;
    [SerializeField] float _scale = 3f;  
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _label;
    [SerializeField] LayoutElement _containerLayoutElement;


    public void Set(BindingDefinition binding)
    {
        if (binding == null || binding.icon == null) return;


        _icon.sprite = binding.icon;
        _label.text = binding.label;

        if (_icon.sprite != null)
        {
            float spriteWidth = _icon.sprite.rect.width;
            float spriteHeight = _icon.sprite.rect.height;
            float scale = 3f;

            float targetWidth = spriteWidth * scale;
            float targetHeight = spriteHeight * scale;
            

            // Update the IconContainer LayoutElement to match the icon
            if (_containerLayoutElement != null)
            {
                _containerLayoutElement.preferredWidth = targetWidth;
                _containerLayoutElement.preferredHeight = targetHeight;
            }
        }
    }
    
}

