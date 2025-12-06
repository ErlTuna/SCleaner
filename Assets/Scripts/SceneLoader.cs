using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SceneReference loadingScreenRef;
    [SerializeField] SceneReference persistentSceneRef;

    [Header("Event Channels")]
    [SerializeField] SceneRequestEventChannelSO sceneRequestEventChannel;
    [SerializeField] VoidEventChannelSO sceneLoadEventChannel;
    [SerializeField] SceneDatabaseSO _sceneDatabaseSO;

    [Header("Internal")]
    readonly Queue<SceneLoaderRequest> _queue = new();
    bool _isProcessing;
    readonly HashSet<SceneReference> loadedsceneReferences = new();

    void OnEnable()
    {
        sceneRequestEventChannel.OnEventRaised += HandleSceneRequest;
    }

    void OnDisable()
    {
        sceneRequestEventChannel.OnEventRaised -= HandleSceneRequest;
    }


    void Awake()
    {    
        // Add persistent scene as loaded (active on startup)
        if (!string.IsNullOrEmpty(persistentSceneRef.SceneName))
        {
            //loadedScenes.Add(persistentSceneRef.SceneName);
            loadedsceneReferences.Add(persistentSceneRef);
            Debug.Log("Added persistent scene to the list.");
        }
    }

    void HandleSceneRequest(SceneLoaderRequest req)
    {
        if (IsRequestValidToEnqueue(req) != true)
        {
            req.OnCompleted.Invoke();
            return;
        } 

        EnqueueRequest(req);
    }

    void EnqueueRequest(SceneLoaderRequest request)
    {
        _queue.Enqueue(request);

        // If nothing is running, start draining the queue
        if (_isProcessing == false)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        _isProcessing = true;
        bool needToShowLoadingScreen = false;
        bool showingLoadingScreen = false;
        bool hasSceneToSetActive = false;
        SceneReference sceneToSetActive = default;

        while (_queue.Count > 0)
        {
            SceneLoaderRequest req = _queue.Dequeue();
            if (req.ShowLoadingScreen)
                needToShowLoadingScreen = true;

            if (req.SetSceneActive)
            {
                sceneToSetActive = req.SceneReference;
                hasSceneToSetActive = true;
            }

            if (needToShowLoadingScreen && showingLoadingScreen != true)
            {
                showingLoadingScreen = true;
                yield return TryLoadScene(loadingScreenRef);
            }
                
            yield return ProcessRequest(req);
            req.OnCompleted?.Invoke();
        }

        if (hasSceneToSetActive) TrySetActiveScene(sceneToSetActive.SceneName);
        _isProcessing = false;

        if (showingLoadingScreen)
            yield return TryUnloadScene(loadingScreenRef);

        //PrintLoadedSceneReferences();

    }


    IEnumerator ProcessRequest(SceneLoaderRequest req)
    {
        if (IsRequestValidToProcess(req) != true)
        {
            req.OnCompleted.Invoke();
            yield break;
        }

        switch (req.RequestType)
        {
            case SceneLoaderRequestType.LOAD_SINGLE:
                yield return TryLoadScene(req.SceneReference);
                break;

            case SceneLoaderRequestType.UNLOAD_SINGLE:
                yield return TryUnloadScene(req.SceneReference);
                break;

            case SceneLoaderRequestType.RELOAD_SINGLE:
                yield return TryReloadScene(req.SceneReference);
                break;

            case SceneLoaderRequestType.UNLOAD_TYPE_UI:
                yield return TryUnloadScenesOfType(req.RequestType);
                break;

        }         
    }

    IEnumerator TryReloadScene(SceneReference sceneRef)
    {
        yield return StartCoroutine(TryUnloadScene(sceneRef));
        yield return StartCoroutine(TryLoadScene(sceneRef));

        Debug.Log("Successfuly reloaded scene : " + sceneRef.SceneName);
        //loadedScenes.Add(sceneRef.SceneName);
    }

    IEnumerator TryLoadScene(SceneReference sceneRef)
    {    
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneRef.SceneName, LoadSceneMode.Additive);
        while (!op.isDone)
            yield return null;

        Debug.Log("Successfuly loaded scene : " + sceneRef.SceneName);
        //loadedScenes.Add(sceneRef.SceneName);
        loadedsceneReferences.Add(sceneRef);
    }
    
    IEnumerator TryUnloadScene(SceneReference sceneRef)
    {
        Debug.Log("Attempting to unload scene : " + sceneRef.SceneName);
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneRef.SceneName);
        while (!op.isDone)
            yield return null;

        Debug.Log("Successfuly unloaded scene : " + sceneRef.SceneName);
        //loadedScenes.Remove(sceneRef.SceneName);
        loadedsceneReferences.Remove(sceneRef);
    }

    IEnumerator TryUnloadScenesOfType(SceneLoaderRequestType requestType)
    {
        Debug.Log($"Attempting to unload multiple scenes as per request {requestType}");

        switch (requestType)
        {
            case SceneLoaderRequestType.UNLOAD_TYPE_UI:
            var scenesToUnload = GetLoadedScenesOfType(SceneType.UI);

            if (scenesToUnload.Count == 0)
            {
                Debug.Log("No scenes of this type were loaded.");
                yield break;
            }

            yield return UnloadSceneList(scenesToUnload);
            break;
        }
    }

    IEnumerator UnloadSceneList(List<SceneReference> scenes)
    {
        foreach (var sceneRef in scenes)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneRef.SceneName);
            while (!op.isDone)
                yield return null;

            loadedsceneReferences.Remove(sceneRef);
        }
    }


    void TrySetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid()) SceneManager.SetActiveScene(scene);

        else
            Debug.LogWarning($"Cannot set active scene: Scene '{sceneName}' is not loaded.");
    }


    // ------------------------------------------------
    // VALIDATION METHODS
    // ------------------------------------------------

    // Initial checks for non-operational failures (i.e : invalid name/missing in database)
    // Done before the request is queued
    bool IsRequestValidToEnqueue(SceneLoaderRequest sceneReq)
    {
        // Special case : Does not require a scene reference if the request is asking to unload all UI type scenes
        if (sceneReq.RequestType == SceneLoaderRequestType.UNLOAD_TYPE_UI && sceneReq.SceneReference == null)
            return true;

        if (string.IsNullOrEmpty(sceneReq.SceneReference.SceneName))
        {
            Debug.LogWarning($"Invalid scene name in the given request. Given scene name {sceneReq.SceneReference.SceneName}");
            return false;
        }

        if (_sceneDatabaseSO.ContainsScene(sceneReq.SceneReference.SceneName) != true)
        {
            Debug.LogWarning($"Scene does not exist in the database. Check the provided scene name or add it to the database. Given scene name {sceneReq.SceneReference.SceneName}");
            return false;
        }

        return true;
    }



    // Cecks for operational failures (i.e : invalid name/missing in database)
    // Done right before the request in the queue is processed.
    bool IsRequestValidToProcess(SceneLoaderRequest request)
    {
        if (IsLoaded(request.SceneReference) && request.RequestType == SceneLoaderRequestType.LOAD_SINGLE)
        {
            Debug.LogWarning($"Trying to load an already loaded scene without a RELOAD request. Given scene name {request.SceneReference.SceneName}");
            return false;
        }

        if (IsLoaded(request.SceneReference) != true && request.RequestType == SceneLoaderRequestType.UNLOAD_SINGLE)
        {
            Debug.LogWarning($"Trying to unload a scene that is not loaded. Given scene name {request.SceneReference.SceneName}");
            return false;
        }

        if (IsLoaded(request.SceneReference) != true && request.RequestType == SceneLoaderRequestType.RELOAD_SINGLE)
        {
            Debug.LogWarning("Trying to reload a scene that is not loaded");
            return false;
        }
        
        return true;
    }


    bool IsLoaded(SceneReference sceneRef)
    {       
        return sceneRef != null && loadedsceneReferences.Contains(sceneRef);
    }



    List<SceneReference> GetLoadedScenesOfType(SceneType type)
    {
        var result = new List<SceneReference>();

        foreach (var sceneReference in loadedsceneReferences)
        {
            if (sceneReference.SceneType == type)
                result.Add(sceneReference);
        }

        return result;
    }

    void PrintLoadedSceneReferences()
    {
        foreach (var sceneReference in loadedsceneReferences)
        {
            Debug.Log("Currently loaded scenes : " + sceneReference.SceneName);
        }
    }

}
