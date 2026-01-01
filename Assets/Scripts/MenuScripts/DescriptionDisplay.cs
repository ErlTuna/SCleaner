using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText;

    GameObject _lastSelected;

    void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        if (current == _lastSelected)
            return;

        _lastSelected = current;

        if (current == null)
        {
            descriptionText.text = "";
            return;
        }

        if (current.TryGetComponent<IDescriptionProvider>(out var provider))
        {
            descriptionText.text = provider.Description;
        }
        else
        {
            descriptionText.text = "";
        }
    }
}
