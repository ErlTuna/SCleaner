using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] SceneRequestEventChannelSO _sceneRequestEventChannel;
    IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync("Loading_Screen", LoadSceneMode.Additive);

        // Load persistent scene
        yield return SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);

        // Load main menu
        yield return StartCoroutine(LoadMainMenu());

        //yield return new WaitForSeconds(2f);

        yield return SceneManager.UnloadSceneAsync("Loading_Screen");
        
        yield return SceneManager.UnloadSceneAsync("Bootstrap");

    }

    IEnumerator LoadMainMenu()
    {
        bool done = false;
        SceneReference mainMenuSceneRef = new()
        {
            SceneName = "Main_Menu_v2",
            SceneType = SceneType.MAIN_MENU
        };

        SceneLoaderRequest request = SceneLoaderRequest.BuildSingleLoadRequest(
            mainMenuSceneRef,
            false,
            true,
            () => done = true
        );

        _sceneRequestEventChannel.RaiseEvent(request);

        yield return new WaitUntil(() => done);
        GameManager.Instance.SetGameState(GameState.IN_MAIN_MENU);
    }


}

/*
var request = new SceneLoaderRequest
    {
        SceneReference = new SceneReference
        {
            SceneName = "Main_Menu_v2",
            SceneType = SceneType.MAIN_MENU
        },
        SetSceneActive = true,
        ShowLoadingScreen = false,
        requestType = SceneLoaderRequestType.LOAD_SINGLE,
        OnCompleted = () => done = true
    };
*/
