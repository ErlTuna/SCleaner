using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IMenuItem
{

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Deselected.");
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        GameManager.Instance.SetGameState(GameState.RETURNING_TO_MAIN_MENU);
        
    }
}
