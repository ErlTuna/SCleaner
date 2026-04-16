using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance {get; private set;}

    
    void Awake(){
        
        if (Instance != null && Instance != this){
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    void Update()
    {
        if (PlayerInputManager.Instance.MenuOpenInput &&
            GameManager.Instance.CurrentState == GameState.PLAYING)
        {
            Pause();
        }
    }


    void Pause()
    {
        Debug.Log("Game paused");
        Time.timeScale = 0f;
        //PlayerInputManager.Instance.ToggleMouseInput(false);
        PlayerInputManager.Instance.EnableMouseCursor(true);
        PlayerInputManager.Instance.SwitchToUIActionMap();

        GameManager.Instance.SetGameState(GameState.PAUSED);
    }

    public void Resume()
    {
        Debug.Log("Game unpaused.");
        Time.timeScale = 1f;
        PlayerInputManager.Instance.ToggleMouseInput(true);
        PlayerInputManager.Instance.EnableMouseCursor(false);
        PlayerInputManager.Instance.SwitchToGameplayActionMap();

        GameManager.Instance.SetGameState(GameState.PLAYING);
    }


    
}
