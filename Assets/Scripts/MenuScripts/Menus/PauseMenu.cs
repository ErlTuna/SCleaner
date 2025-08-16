using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        OnGameResumed?.Invoke();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        OnGamePaused?.Invoke();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused=true;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Game2MainMenu()
    {
        Resume();
        SceneManager.LoadScene("Main_Menu");
    }
}
