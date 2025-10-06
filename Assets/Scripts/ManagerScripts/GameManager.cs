using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject DeathScreenUI;

    public int EnemyCounter = 0;
    public int BulletCounter = 0;
    public int KillCounter = 0;
    
    void OnEnable(){
        
        //IWeapon.OnWeaponFireEvent += BulletStat;
        //PlayerHealth.onPlayerDeath += EndGame;
        BugHealth.OnDeathEvent += KillStat;
    }

    void OnDisable(){
        //IWeapon.OnWeaponFireEvent += BulletStat;
        //PlayerHealth.onPlayerDeath += EndGame;
        BugHealth.OnDeathEvent -= KillStat;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Game2MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("odalar1");
        Time.timeScale = 1f;
    }

    public void EnemyStat()
    {
        EnemyCounter++;
    }

    public void BulletStat()
    {
        BulletCounter++;
    }

    public void KillStat()
    {
        KillCounter++;
        /*if(KillCounter > EnemyCounter)
        {
            KillCounter = EnemyCounter;
        }*/
    }

    public Text EnemyStatText;
    public Text KillStatText;
    public Text BulletStatText;
   
    public void EndGame()
    {
        DeathScreenUI.SetActive(true);
        Time.timeScale = 0f;
        EnemyStatText.text = EnemyCounter.ToString();
        KillStatText.text = KillCounter.ToString();
        BulletStatText.text = BulletCounter.ToString();
    }


}
