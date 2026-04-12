using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuInventorySection : MonoBehaviour, IMenuSection
{
    [SerializeField] RectTransform _rootRect;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] List<InventoryUIGroup> _inventoryGroups;
    public bool IsVisible { get; set;}

    public void Show()
    {
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            PlayerInputManager.Instance.ToggleMouseInput(true);

            foreach (InventoryUIGroup group in _inventoryGroups)
            {
                if (group.IsEmpty() != true)
                {
                    UISelector.instance.SetSelected(group.GetDefaultSelected());
                    Debug.Log("Set selected in inventory to : " + EventSystem.current.currentSelectedGameObject);
                    break;
                }
            }
            IsVisible = true;
        }
    }

    public void Hide()
    {
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            PlayerInputManager.Instance.ToggleMouseInput(false);
            UISelector.instance.SetSelected(null);
            InventorySelectedItemDisplay.Instance.Reset();
            IsVisible = false;
        }
    }
}
