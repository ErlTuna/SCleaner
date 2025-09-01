using TMPro.EditorUtilities;
using UnityEngine;

public class UI_Crosshair : MonoBehaviour
{

    [SerializeField] RectTransform rectTransform;
    float defaultScale = 1f;
    float currentScale = 1f;
    float scalingFactor = 0.2f;
    float scalingFactorFloor = 0.05f;

    void OnEnable()
    {
        //IWeapon.OnWeaponFiredVisuals += EnlargeCrosshair;
    }

    void OnDisable()
    {
        //IWeapon.OnWeaponFiredVisuals -= EnlargeCrosshair;
    }

    void Update()
    {
        rectTransform.position = PlayerInputManager.instance.PointerInput;
    }

    /*void EnlargeCrosshair()
    {
        Debug.Log("awesome");
        currentScale = Mathf.Clamp(currentScale * scalingFactor, 1f, 1.5f);
        if (scalingFactor > scalingFactorFloor)
        {
            scalingFactor *= .5f;
        }
        rectTransform.localScale *= currentScale;
    }*/
}
