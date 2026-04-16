using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnGameStart;
    public static Action OnLevelLoaded;
    public static Action<SceneReference> OnLevelLoadedMusicRequest;
    public static Action OnLevelOver;
    public static Action OnGameOver;
    public static Action OnGameCompleted;
    public static Action OnGameOverCameraMovement;
    public static Action OnGameOverShowGameOverMenu;

    [SerializeField] int _currentGameplayLevelIndex;
    [SerializeField] SceneReference _currentGameplaySceneRef;
    
    [SerializeField] SceneDatabaseSO _sceneDatabase;
    [SerializeField] SceneRequestEventChannelSO _sceneRequestEventChannel;
    [SerializeField] GameStateEventChannel _gameStateChangedEventChannel;

    GameState _previousState;
    [SerializeField] GameState _currentState;
    public GameState CurrentState => _currentState;
    public SceneReference CurrentGameplaySceneRef => _currentGameplaySceneRef;
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
                OnLevelLoadedMusicRequest?.Invoke(_sceneDatabase.MainMenuRef);
                //PlayerInputManager.Instance.ToggleMouseInput(false);
                PlayerInputManager.Instance.SwitchToUIActionMap();
                break;

            case GameState.LOADING_GAME:
                StartCoroutine(StartNewGame());
                break;
                
            case GameState.LOADING_NEXT_GAMEPLAY_LEVEL:
                StartCoroutine(StartLoadingNextLevel());
                break;            

            case GameState.PLAYING when _previousState == GameState.PAUSED:
                OnUnpause();
                break;

            case GameState.PAUSED when _previousState == GameState.PLAYING:
                OnPause();
                break;

            case GameState.PLAYER_DEFEAT:
                StartCoroutine(PlayerDefeatSequence());
                break;

            case GameState.LEVEL_COMPLETED:
                OnLevelFinished();
                break;

            case GameState.RETURNING_TO_MAIN_MENU :
                StartCoroutine(ReturnToMainMenu());
                break;
                
            case GameState.SHUTTING_DOWN :
                OnQuit();
                break;
        }

        //if (_gameStateChangedEventChannel != null)
            //_gameStateChangedEventChannel.RaiseEvent(_currentState);
        
    }

    IEnumerator StartNewGame()
    {
        int pendingRequests = 0;
        _currentGameplayLevelIndex = 0;

        SceneReference testSceneRef = _sceneDatabase.TestSceneRef;
        _currentGameplaySceneRef = _sceneDatabase.GetLevel(_currentGameplayLevelIndex);
        //SceneReference _currentGameplaySceneRef = _sceneDatabase.TestSceneRef;
        SceneReference gameplayUISceneRef = _sceneDatabase.GameplayUIRef;
        SceneReference mainMenuSceneRef = _sceneDatabase.MainMenuRef;
        SceneReference pauseMenuSceneRef = _sceneDatabase.PauseMenuRef;
        SceneReference gameOverUISceneRef = _sceneDatabase.GameOverUIRef;

        
        //SceneLoaderRequest unloadMainMenuReq = SceneLoaderRequest.BuildUnloadByTypeRequest(SceneType.MAIN_MENU).OnComplete(() => --pendingRequests);
        SceneLoaderRequest unloadMainMenuReq = SceneLoaderRequest.BuildSingleUnloadRequest(mainMenuSceneRef).OnComplete(() => --pendingRequests);
        SceneLoaderRequest gameplayUIReq = SceneLoaderRequest.BuildSingleLoadRequest(gameplayUISceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(true).
                                                     OnComplete(() => --pendingRequests);

        SceneLoaderRequest pauseMenuLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(pauseMenuSceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(false).
                                                     OnComplete(() => --pendingRequests);

        SceneLoaderRequest gameOverUILoadReq = SceneLoaderRequest.BuildSingleLoadRequest(gameOverUISceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(false).
                                                     OnComplete(() => --pendingRequests);                                             

        SceneLoaderRequest testSceneLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(testSceneRef).
                                                     SetActive(true).
                                                     WithLoadingScreen(false). // Set this to true later
                                                     OnComplete(() => --pendingRequests);
                                                     

        SceneLoaderRequest gameplayLevelLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(_currentGameplaySceneRef).
                                                     SetActive(true).
                                                     WithLoadingScreen(false). // Set this to true later
                                                     OnComplete(() => --pendingRequests);


        List<SceneLoaderRequest> requests = new() {unloadMainMenuReq, gameplayUIReq, pauseMenuLoadReq, gameOverUILoadReq, gameplayLevelLoadReq};
        pendingRequests = requests.Count;

        foreach (var req in requests)
            _sceneRequestEventChannel.RaiseEvent(req);

        yield return new WaitUntil(() => pendingRequests <= 0);
        PlayerInputManager.Instance.ToggleMouseInput(true);
        PlayerInputManager.Instance.EnableMouseCursor(false);
        SetGameState(GameState.PLAYING);
        PlayerInputManager.Instance.SwitchToGameplayActionMap();

        OnLevelLoaded?.Invoke();
        OnGameStart?.Invoke();
        OnLevelLoadedMusicRequest?.Invoke(_currentGameplaySceneRef);
    }

    IEnumerator StartLoadingNextLevel()
    {
        yield return null;
        int pendingRequests = 0;
        _currentGameplayLevelIndex += 1;


        SceneReference nextGameplaySceneRef = _sceneDatabase.GetLevel(_currentGameplayLevelIndex);
        SceneReference gameplayUISceneRef = _sceneDatabase.GameplayUIRef;
        SceneReference pauseMenuSceneRef = _sceneDatabase.PauseMenuRef;
        SceneReference gameOverUISceneRef = _sceneDatabase.GameOverUIRef;

        // Unload current gameplay scene
        SceneLoaderRequest unloadCurrentGameplayScene = SceneLoaderRequest.BuildSingleUnloadRequest(_currentGameplaySceneRef).OnComplete(() => --pendingRequests);

        // Unload menus and UI
        SceneLoaderRequest unloadGameplayUI = SceneLoaderRequest.BuildSingleUnloadRequest(gameplayUISceneRef).OnComplete(() => --pendingRequests);
        SceneLoaderRequest unloadPauseMenu = SceneLoaderRequest.BuildSingleUnloadRequest(pauseMenuSceneRef).OnComplete(() => --pendingRequests);
        SceneLoaderRequest unloadGameOverUI = SceneLoaderRequest.BuildSingleUnloadRequest(gameOverUISceneRef).OnComplete(() => --pendingRequests);

        // Reload the menu and UI
        SceneLoaderRequest gameplayUIReq = SceneLoaderRequest.BuildSingleLoadRequest(gameplayUISceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(true).
                                                     OnComplete(() => --pendingRequests);

        SceneLoaderRequest pauseMenuLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(pauseMenuSceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(false).
                                                     OnComplete(() => --pendingRequests);

        SceneLoaderRequest gameOverUILoadReq = SceneLoaderRequest.BuildSingleLoadRequest(gameOverUISceneRef).
                                                     SetActive(false).
                                                     WithLoadingScreen(false).
                                                     OnComplete(() => --pendingRequests);  


        // Load the next gameplay scene
        SceneLoaderRequest gameplayLevelLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(nextGameplaySceneRef).
                                                     SetActive(true).
                                                     WithLoadingScreen(true).
                                                     OnComplete(() => --pendingRequests);


        List<SceneLoaderRequest> requests = new() {unloadCurrentGameplayScene, unloadGameplayUI, unloadPauseMenu, unloadGameOverUI, gameplayUIReq, pauseMenuLoadReq, gameOverUILoadReq, gameplayLevelLoadReq};

        foreach (var req in requests)
            _sceneRequestEventChannel.RaiseEvent(req);

        yield return new WaitUntil(() => pendingRequests <= 0);

        PlayerInputManager.Instance.ToggleMouseInput(true);
        PlayerInputManager.Instance.EnableMouseCursor(false);
        PlayerInputManager.Instance.SwitchToGameplayActionMap();

        OnLevelLoaded?.Invoke();
        OnGameStart?.Invoke();
        OnLevelLoadedMusicRequest?.Invoke(_currentGameplaySceneRef);

        _currentGameplaySceneRef = nextGameplaySceneRef;

        LootManager.Instance.EnableLootDrops(false);

        SetGameState(GameState.PLAYING);
    }

    IEnumerator ReturnToMainMenu()
    {
        int pendingRequests = 0;
        
        SceneReference mainMenuSceneRef = _sceneDatabase.MainMenuRef;
        SceneReference pauseMenuRef = _sceneDatabase.PauseMenuRef;

        SceneLoaderRequest mainMenuLoadReq = SceneLoaderRequest.BuildSingleLoadRequest(mainMenuSceneRef).WithLoadingScreen(true).SetActive(true).OnComplete(() => --pendingRequests);
        SceneLoaderRequest pauseMenuUnloadReq = SceneLoaderRequest.BuildSingleUnloadRequest(pauseMenuRef).OnComplete(() => --pendingRequests);
        SceneLoaderRequest unloadAllUIReq = SceneLoaderRequest.BuildUnloadByTypeRequest(SceneType.UI).OnComplete(() => --pendingRequests);
        SceneLoaderRequest currentGameplaySceneUnloadReq = SceneLoaderRequest.BuildSingleUnloadRequest(_currentGameplaySceneRef).OnComplete(() => --pendingRequests);


        
        List<SceneLoaderRequest> requests = new() {mainMenuLoadReq, pauseMenuUnloadReq, unloadAllUIReq, currentGameplaySceneUnloadReq};
        
        foreach (var req in requests) _sceneRequestEventChannel.RaiseEvent(req);

        yield return new WaitUntil(() => pendingRequests <= 0);
        SetGameState(GameState.IN_MAIN_MENU);
        Time.timeScale = 1f;

        OnLevelLoadedMusicRequest?.Invoke(mainMenuSceneRef);
    }

    void HandleGameOver()
    {
        StartCoroutine(PlayerDefeatSequence());
    }

    void OnPause()
    {
        if (_gameStateChangedEventChannel != null)
            _gameStateChangedEventChannel.RaiseEvent(GameState.PAUSED);
    }

    void OnUnpause()
    {
        if (_gameStateChangedEventChannel != null)
            _gameStateChangedEventChannel.RaiseEvent(GameState.PLAYING);

        PauseManager.Instance.Resume();

    }

    void OnQuit() => Application.Quit();

    IEnumerator PlayerDefeatSequence()
    {

        OnGameOver?.Invoke();
        PlayerInputManager.Instance.OnPlayerDefeat();
        bool cameraOnPlayer = false;

        // small lambda method
        CameraTargetFollow.OnCameraMovedToPlayer += () => cameraOnPlayer = true;
        OnGameOverCameraMovement.Invoke();
        yield return new WaitUntil(() => cameraOnPlayer == true);
        CameraTargetFollow.OnCameraMovedToPlayer -= () => cameraOnPlayer = true;

        PlayerInputManager.Instance.SwitchToUIActionMap();
        OnGameOverShowGameOverMenu?.Invoke();
    }

    void OnLevelFinished()
    {
        PlayerInputManager.Instance.DisableGameplayActionMap();
        PlayerInputManager.Instance.EnableMouseCursor(true);
        PlayerInputManager.Instance.SwitchToUIActionMap();
        SceneReference nextGameplaySceneRef = _sceneDatabase.GetLevel(_currentGameplayLevelIndex + 1);

        if (nextGameplaySceneRef == null)
        {
            //Debug.Log("There is no gameplay level to load.");
            OnGameCompleted?.Invoke();
            return;
        }


        OnLevelOver?.Invoke();
    }

}


