using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BindingHintItem_v2 : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private RectTransform _root;   // HintItem RectTransform
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _label;

    [Header("Layout")]
    [SerializeField] private float _iconScale = 3f;
    [SerializeField] private float _spacing = 6f;

    public void Set(BindingDefinition binding)
    {
        if (binding == null || binding.icon == null)
            return;

        // -----------------------------
        // 1. Assign data
        // -----------------------------
        _icon.sprite = binding.icon;
        _label.text = binding.label;

        // -----------------------------
        // 2. Calculate icon size
        // -----------------------------
        float iconWidth  = binding.icon.rect.width  * _iconScale;
        float iconHeight = binding.icon.rect.height * _iconScale;

        // -----------------------------
        // 3. Measure text (TMP)
        // -----------------------------
        _label.ForceMeshUpdate();

        float labelWidth  = _label.preferredWidth;
        float labelHeight = _label.preferredHeight;

        // -----------------------------
        // 4. Compute final sizes
        // -----------------------------
        float height = Mathf.Max(iconHeight, labelHeight);
        float width  = iconWidth + _spacing + labelWidth;

        // -----------------------------
        // 5. Resize HintItem
        // -----------------------------
        _root.sizeDelta = new Vector2(width, height);

        // -----------------------------
        // 6. Size & position icon
        // -----------------------------
        RectTransform iconRT = _icon.rectTransform;
        iconRT.sizeDelta = new Vector2(iconWidth, iconHeight);
        iconRT.anchoredPosition = new Vector2(
            iconWidth * 0.5f,
            height * 0.5f
        );

        // -----------------------------
        // 7. Position label
        // -----------------------------
        RectTransform labelRT = _label.rectTransform;
        labelRT.anchoredPosition = new Vector2(
            iconWidth + _spacing,
            (height - labelHeight) * 0.5f
        );
    }
}

