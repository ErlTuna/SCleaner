using UnityEngine.UI;
using UnityEngine;

// this class is no longer used, refer to MenuAnimationsManager

public class SelectionBlinkHandler : MonoBehaviour
{

    [SerializeField] Image _selectionIndicator;
    float blinkInterval = 0.35f;
    float lastBlinkTime = 0f;
    bool isSelected = false;
    bool isIndicatorVisible = false;

    void Update()
    {
        Blink();
    }

    void OnEnable()
    {
        isSelected = true;
        lastBlinkTime = Time.time;
        isIndicatorVisible = true;
        _selectionIndicator.enabled = true;
    }

    void OnDisable()
    {
        isSelected = false;
        lastBlinkTime = Time.time;
        isIndicatorVisible = false;
        _selectionIndicator.enabled = false;
    }

    public void Blink()
    {
        if (isSelected)
        {
            // if we have not blinked since the last time
            if (Time.time - lastBlinkTime >= blinkInterval)
            {
                isIndicatorVisible = !isIndicatorVisible;
                _selectionIndicator.enabled = isIndicatorVisible;
                lastBlinkTime = Time.time;
            }
        }

    }
}
