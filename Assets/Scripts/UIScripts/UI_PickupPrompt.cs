using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PickupPrompt : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] Transform target;
    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = false;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }



    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position;
        transform.rotation = Quaternion.identity;

    }

}
