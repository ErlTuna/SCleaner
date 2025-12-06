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
        if (PlayerInputManager.Instance.MenuOpenInput && GameManager.Instance.CurrentState == GameState.PLAYING)
        {
            GameManager.Instance.SetGameState(GameState.PAUSED);
        }

        else if (PlayerInputManager.Instance.MenuCloseInput && GameManager.Instance.CurrentState == GameState.PAUSED)
        {
            GameManager.Instance.SetGameState(GameState.PLAYING);
        }
    }
}
