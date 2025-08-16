using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public bool IsPaused;

    void Awake(){
        
        if (instance != null && instance != this){
            Destroy(this);
        }
        else
            instance = this;  
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        PlayerInputManager.PlayerInput.SwitchCurrentActionMap("UI");
    }

    public void UnPauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        PlayerInputManager.PlayerInput.SwitchCurrentActionMap("Gameplay");
    }
}
