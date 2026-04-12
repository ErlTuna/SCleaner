using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] SceneRequestEventChannelSO _sceneRequestEventChannel;
    [SerializeField] SceneDatabaseSO _sceneDatabase;
    IEnumerator Start()
    {
        //yield return SceneManager.LoadSceneAsync("Loading_Screen", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync(_sceneDatabase.LoadingScreenRef.SceneName, LoadSceneMode.Additive);

        // Load persistent scene
        //yield return SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync(_sceneDatabase.PersistentSceneRef.SceneName, LoadSceneMode.Additive);

        // Load main menu
        yield return StartCoroutine(LoadMainMenu());

        //yield return new WaitForSeconds(2f);

        //yield return SceneManager.UnloadSceneAsync("Loading_Screen");

        yield return SceneManager.UnloadSceneAsync(_sceneDatabase.LoadingScreenRef.SceneName);
        yield return SceneManager.UnloadSceneAsync(_sceneDatabase.BootstrapSceneRef.SceneName);

        //yield return SceneManager.UnloadSceneAsync("Bootstrap");

    }

    IEnumerator LoadMainMenu()
    {
        bool done = false;
        /*
        SceneReference mainMenuSceneRef = new()
        {
            SceneName = "MainMenu_FINAL",
            SceneType = SceneType.MAIN_MENU
        };

        SceneLoaderRequest request = SceneLoaderRequest.BuildSingleLoadRequest(
            mainMenuSceneRef,
            false,
            true,
            () => done = true
        );
        */

        SceneLoaderRequest request = SceneLoaderRequest.BuildSingleLoadRequest(
            _sceneDatabase.MainMenuRef,
            showLoadingScreen : false,
            setActive : true,
            () => done = true
        );

        _sceneRequestEventChannel.RaiseEvent(request);

        yield return new WaitUntil(() => done);
        GameManager.Instance.SetGameState(GameState.IN_MAIN_MENU);
    }


}
