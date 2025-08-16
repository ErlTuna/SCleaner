using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu2Game : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void settingsMenu()
    {
        SceneManager.LoadScene("Settings Menu");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}



 