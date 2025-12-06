using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    void Start()
    {
        if (_animator != null)
            _animator.Play("Loading");
    }
}

/*
IEnumerator LoadLevel()
    {
        AsyncOperation UILoad = SceneManager.LoadSceneAsync("GameplayUI", LoadSceneMode.Additive);
        UILoad.allowSceneActivation = false;

        while (UILoad.progress < 0.9f)
        {
            yield return null;
        }

        UILoad.allowSceneActivation = true;

        while (!UILoad.isDone)
        {
            yield return null;
        }

        AsyncOperation GameLoad = SceneManager.LoadSceneAsync("Test", LoadSceneMode.Additive);
        GameLoad.allowSceneActivation = false;

        while (GameLoad.progress < 0.9f)
        {
            yield return null;
        }        

        GameLoad.allowSceneActivation = true;

        while (!GameLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2.5f);

        // Unload the loading screen scene
        AsyncOperation LoadingScreenUnload = SceneManager.UnloadSceneAsync("Loading_Screen");
        LoadingScreenUnload.allowSceneActivation = false;
    
    }
*/