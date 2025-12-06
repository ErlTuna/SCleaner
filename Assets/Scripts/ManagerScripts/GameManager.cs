using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneRequestEventChannelSO _sceneRequestEventChannel;
    [SerializeField] VoidEventChannelSO _pauseToggleEventChannel;
    [SerializeField] VoidEventChannelSO _gameOverEventChannel;
    public static Action OnGameStart;
    public static Action OnLevelLoaded;
    public static Action OnGameOver;
    public static Action OnGameOverCameraMovement;
    GameState _previousState;
    [SerializeField] GameState _currentState;
    public GameState CurrentState => _currentState; 


    public static GameManager Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }

    public void SetGameState(GameState newState)
    {
        _previousState = _currentState;
        _currentState = newState;

        switch (newState)
        {
            case GameState.IN_MAIN_MENU:
                PlayerInputManager.Instance.ToggleMouseInput(false);
                break;
            case GameState.LOADING_GAME:
                StartCoroutine(StartNewGame());
                break;
            case GameState.PLAYING when _previousState == GameState.PAUSED:
                OnUnpause();
                break;
            case GameState.PAUSED when _previousState == GameState.PLAYING:
                OnPause();
                break;
            case GameState.PLAYER_DEFEAT:
                HandleGameOver();
                break;
            case GameState.RETURNING_TO_MAIN_MENU :
                StartCoroutine(ReturnToMainMenu());
                break;
            case GameState.SHUTTING_DOWN :
                OnQuit();
                break;
        }

        
    }

    IEnumerator StartNewGame()
    {
        int pendingRequests = 0;

        SceneReference testSceneRef = new() { SceneName = "Test", SceneType = SceneType.GAMEPLAY_LEVEL };
        SceneReference gameplayUISceneRef = new() { SceneName = "GameplayUI", SceneType = SceneType.UI };
        SceneReference mainMenuSceneRef = new() { SceneName = "Main_Menu_v2", SceneType = SceneType.MAIN_MENU };
        SceneReference pauseMenuSceneRef = new() { SceneName = "PauseMenu", SceneType = SceneType.UI };
        SceneReference gameOverUIScene = new() { SceneName = "GameOver", SceneType = SceneType.UI };

        

        SceneLoaderRequest mainMenuUnloadReq = SceneLoaderRequest.BuildSingleUnloadRequest(mainMenuSceneRef, false, false, () => --pendingRequests);
        SceneLoaderRequest gamePlayUILoadReq = SceneLoaderRequest.BuildSingleLoadRequest(gameplayUISceneRef, false, false, () => --pendingRequests);
        SceneLoaderRequest pauseMenuLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(pauseMenuSceneRef, false, false, () => --pendingRequests);
        SceneLoaderRequest gameOverUIReq = SceneLoaderRequest.BuildSingleLoadRequest( gameOverUIScene, false, false, () => --pendingRequests);
        SceneLoaderRequest testSceneLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(testSceneRef, true, true, () => --pendingRequests);

        List<SceneLoaderRequest> requests = new() {testSceneLoadReq, gamePlayUILoadReq, pauseMenuLoadReq, gameOverUIReq, mainMenuUnloadReq};
        pendingRequests = requests.Count;

        foreach (var req in requests)_sceneRequestEventChannel.RaiseEvent(req);

        yield return new WaitUntil(() => pendingRequests <= 0);
        PlayerInputManager.Instance.ToggleMouseInput(true);
        SetGameState(GameState.PLAYING);
        PlayerInputManager.Instance.SwitchToGameplayActionMap();

        OnLevelLoaded?.Invoke();
        OnGameStart?.Invoke();
    }

    IEnumerator ReturnToMainMenu()
    {
        int pendingRequests = 0;

        SceneReference mainMenuSceneRef = new() { SceneName = "Main_Menu_v2", SceneType = SceneType.MAIN_MENU };
        SceneReference currentGameplayScene = new() { SceneName = SceneManager.GetActiveScene().name, SceneType = SceneType.GAMEPLAY_LEVEL };
        
        SceneLoaderRequest mainMenuLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(mainMenuSceneRef, true, true, () => --pendingRequests);
        SceneLoaderRequest unloadAllUIReq = SceneLoaderRequest.BuildUnloadByTypeRequest(SceneType.UI, () => --pendingRequests);
        SceneLoaderRequest currentGameplaySceneUnloadReq = SceneLoaderRequest.BuildSingleUnloadRequest(currentGameplayScene, false, false, () => --pendingRequests);
        

        Debug.Log("The current gameplay scene name is : " + currentGameplayScene.SceneName);
        List<SceneLoaderRequest> requests = new() { mainMenuLoadReq, unloadAllUIReq, currentGameplaySceneUnloadReq };
        
        foreach (var req in requests) _sceneRequestEventChannel.RaiseEvent(req);

        yield return new WaitUntil(() => pendingRequests <= 0);
        SetGameState(GameState.IN_MAIN_MENU);
        Time.timeScale = 1f;
    }

    void HandleGameOver()
    {
        Debug.Log("Game over...");
        StartCoroutine(PlayerDefeatSequence());
    }

    void OnPause()
    {
        if (_pauseToggleEventChannel != null)
            _pauseToggleEventChannel.RaiseEvent();

        Debug.Log("Game paused");
        Time.timeScale = 0f;
        PlayerInputManager.Instance.ToggleMouseInput(false);
        PlayerInputManager.Instance.SwitchToUIActionMap();
    }

    void OnUnpause()
    {
        if (_pauseToggleEventChannel != null)
            _pauseToggleEventChannel.RaiseEvent();

        Debug.Log("Game unpaused.");
        Time.timeScale = 1f;
        PlayerInputManager.Instance.ToggleMouseInput(true);
        PlayerInputManager.Instance.SwitchToGameplayActionMap();
    }

    void OnQuit() => Application.Quit();

    IEnumerator PlayerDefeatSequence()
    {
        Debug.Log("Starting game over sequence");

        OnGameOver?.Invoke();
        PlayerInputManager.Instance.OnPlayerDefeat();
        bool cameraOnPlayer = false;

        // small lambda method
        CameraTargetFollow.OnCameraMovedToPlayer += () => cameraOnPlayer = true;
        OnGameOverCameraMovement.Invoke();

        yield return new WaitUntil(() => cameraOnPlayer == true);

        CameraTargetFollow.OnCameraMovedToPlayer -= () => cameraOnPlayer = true;

        // Show game over UI / scene
        if (_gameOverEventChannel != null)
        {
            Debug.Log("Showing game over scene");
            _gameOverEventChannel.RaiseEvent();
        }
    }

}


