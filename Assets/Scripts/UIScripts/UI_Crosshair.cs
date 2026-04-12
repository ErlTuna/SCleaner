using UnityEngine;

public class UI_Crosshair : MonoBehaviour
{

    [SerializeField] RectTransform rectTransform;


    void Update()
    {
        if (PlayerInputManager.Instance == null) return;
        rectTransform.position = PlayerInputManager.Instance.PointerInput;
    }

}
