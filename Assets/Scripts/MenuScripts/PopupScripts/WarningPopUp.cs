using UnityEngine;

public class WarningPopUp : MonoBehaviour, IMenuSection
{
    [SerializeField] GameObject _defaultSelected;
    public bool IsVisible { get; set;}
    public void Show()
    {
        gameObject.SetActive(true);
        UISelector.instance.SetSelected(_defaultSelected);
        IsVisible = false;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        UISelector.instance.SetSelected(null);
        IsVisible = true;
    }
}
