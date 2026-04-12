using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles which bindings appear according to the selected UI element's providings.
// EX : Button -> [ENTER] SUBMIT and [ESC] BACK.
// Ex : Slider -> [LEFT ARROW - RIGHT ARROW] ADJUST SLIDER, [ESC] BACK.
public class BindingHintManager : MonoBehaviour
{
    [SerializeField] Transform _container;
    [SerializeField] GameObject _itemPrefab;
    GameObject _lastSelected;
    Dictionary<BindingId, BindingHintItem> _activeItems = new();
    List<BindingHintItem> _pooledItems = new();


    void Update()
    {
        if (EventSystem.current == null) return;
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;
        if (selected == _lastSelected) return;

        _lastSelected = selected;
        Refresh();
    }

    void Refresh()
    {   
        // _lastSelected == null|| 
        if (_lastSelected.TryGetComponent(out BindingHintProvider provider) == false)
        {
            DeactivateAll();
            return;
        }

        int index = 0;
        var unused = new HashSet<BindingId>(_activeItems.Keys);

        foreach (var binding in provider.bindings)
        {

            if (_activeItems.TryGetValue(binding.id, out BindingHintItem item))
            {
                item.Set(binding);
                item.gameObject.SetActive(true);
            }
            else
            {
                item = GetPooledItem();
                item.Set(binding);
                item.gameObject.SetActive(true);
                _activeItems.Add(binding.id, item);
            }

            item.transform.SetSiblingIndex(index);

            unused.Remove(binding.id);
            index++;
        }

        foreach (var id in unused)
        {
            var item = _activeItems[id];
            item.gameObject.SetActive(false);
            _pooledItems.Add(item);
            _activeItems.Remove(id);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            _container as RectTransform
        );
    }

    BindingHintItem GetPooledItem()
    {
        if (_pooledItems.Count > 0)
        {
            BindingHintItem item = _pooledItems[^1];
            _pooledItems.RemoveAt(_pooledItems.Count - 1);
            return item;
        }

        return Instantiate(_itemPrefab, _container).GetComponent<BindingHintItem>();
    }



    void DeactivateAll()
    {
        foreach (BindingHintItem item in _activeItems.Values)
        {
            item.gameObject.SetActive(false);
            _pooledItems.Add(item);
        }

        _activeItems.Clear();
    }


}