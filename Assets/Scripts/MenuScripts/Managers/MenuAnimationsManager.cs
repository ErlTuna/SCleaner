using UnityEngine.UI;
using UnityEngine;


// singleton responsible for menu transition animations
// on previous iterations when the menu had transition animations, this class was the one handling them 
// now, it only works for handling the option selection indicator blinks
public class MenuAnimationsManager : MonoBehaviour
{

    [SerializeField] Animator _animator;
    public static MenuAnimationsManager instance;
    public Image selectionIndicator;
    public float blinkInterval = 0.35f;
    float lastBlinkTime = 0f;
    bool isIndicatorVisible = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Update()
    {
        BlinkSelectionIndicator();
    }

    // disables previous selection indicator and enables new one
    public void OptionSelected(Image newIndicator)
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.enabled = false;
        }

        if (newIndicator != null)
        {
            selectionIndicator = newIndicator;
            selectionIndicator.enabled = true;
        }
        else
        {
            Debug.LogWarning("New selectionIndicator is null!");
        }

    }

    public void OptionDeselected()
    {
        if (selectionIndicator != null)
            selectionIndicator.enabled = false;
    }

    public void ReceiveAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void BlinkSelectionIndicator()
    {
        if (selectionIndicator == null) return;


        // toggles indicator depending on how much time has passed since last toggle
        if (Time.unscaledTime - lastBlinkTime >= blinkInterval)
        {
            isIndicatorVisible = !isIndicatorVisible;
            selectionIndicator.enabled = isIndicatorVisible;
            lastBlinkTime = Time.unscaledTime;
        }
    }


}
