using UnityEngine;
using UnityEngine.UI;

public class SliderScrollLink : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] Slider _slider;

    void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        _scrollRect.verticalNormalizedPosition = value;
    }
}