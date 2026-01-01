using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, ISelectHandler
{
    public ScrollRect scrollRect;

    public void OnSelect(BaseEventData eventData)
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.content.localPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(transform.position);
    }
}
