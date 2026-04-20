using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _track;
    [SerializeField] RectTransform _handle;

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _track,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPos
        );

        float halfHeight = _track.rect.height / 2f;

        float clampedY = Mathf.Clamp(localPos.y, -halfHeight, halfHeight);

        _handle.anchoredPosition = new Vector2(_handle.anchoredPosition.x, clampedY);
    }
}