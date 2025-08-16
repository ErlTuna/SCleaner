using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


// helper class for updating event system's current selected game object
// waits a frame before changing selected game object
public class UISelector : MonoBehaviour
{
    public static UISelector instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void SetSelected(GameObject selectionTarget)
    {
        StartCoroutine(DelayedSelect(selectionTarget));
    }

    private IEnumerator DelayedSelect(GameObject selectionTarget)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(selectionTarget);
    }

}
