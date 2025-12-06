using System;
using System.Collections.Generic;

public class SceneLoaderRequest
{
    public SceneReference SceneReference { get; private set; } // optional depending on type
    public bool ShowLoadingScreen { get; private set; }
    public bool SetSceneActive { get; private set; }
    public SceneLoaderRequestType RequestType { get; private set; }
    public Action OnCompleted { get; private set; } // optional

    private SceneLoaderRequest() { } // private constructor

    public static SceneLoaderRequest BuildSingleLoadRequest(SceneReference scene, bool showLoadingScreen = false, bool setActive = false, Action onCompleted = null)
    {
        return new SceneLoaderRequest
        {
            RequestType = SceneLoaderRequestType.LOAD_SINGLE,
            SceneReference = scene,
            ShowLoadingScreen = showLoadingScreen,
            SetSceneActive = setActive,
            OnCompleted = onCompleted
        };
    }

    public static SceneLoaderRequest BuildSingleUnloadRequest(SceneReference scene, bool showLoadingScreen = false, bool setActive = false, Action onCompleted = null)
    {
        return new SceneLoaderRequest
        {
            RequestType = SceneLoaderRequestType.UNLOAD_SINGLE,
            SceneReference = scene,
            ShowLoadingScreen = showLoadingScreen,
            SetSceneActive = setActive,
            OnCompleted = onCompleted
        };
    }

    public static SceneLoaderRequest BuildSingleReloadRequest(SceneReference scene, bool showLoadingScreen = false, bool setActive = false, Action onCompleted = null)
    {
        return new SceneLoaderRequest
        {
            RequestType = SceneLoaderRequestType.RELOAD_SINGLE,
            SceneReference = scene,
            ShowLoadingScreen = showLoadingScreen,
            SetSceneActive = setActive,
            OnCompleted = onCompleted
        };
    }

    public static SceneLoaderRequest BuildUnloadByTypeRequest(SceneType sceneType, Action onCompleted = null)
    {
        switch (sceneType)
        {
            case SceneType.UI:
                return new SceneLoaderRequest
                {
                    RequestType = SceneLoaderRequestType.UNLOAD_TYPE_UI,
                    OnCompleted = onCompleted
                };

            default:
                return null;
            // future cases...    
        }
    }

    

    // Add other factory methods for other request types
}

public enum SceneLoaderRequestType
{
    LOAD_SINGLE,
    UNLOAD_SINGLE,
    RELOAD_SINGLE,
    UNLOAD_TYPE_UI
}