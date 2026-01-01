using System;
using System.Collections.Generic;

public class SceneLoaderRequest
{
    public SceneReference SceneReference { get; internal set; } // optional depending on type
    public bool ShowLoadingScreen { get; internal set; }
    public bool SetSceneActive { get; internal set; }
    public SceneLoaderRequestType RequestType { get; internal set; }
    public Action OnCompleted { get; internal set; } // optional

    internal SceneLoaderRequest() { }

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
            case SceneType.MAIN_MENU:
                return new SceneLoaderRequest
                {
                    RequestType = SceneLoaderRequestType.UNLOAD_MAIN_MENU,
                    OnCompleted = onCompleted
                };
            case SceneType.GAMEPLAY_LEVEL:
                return new SceneLoaderRequest
                {
                    RequestType = SceneLoaderRequestType.UNLOAD_CURRENT_GAME_LEVEL,
                    OnCompleted = onCompleted
                };
            default:
                return null;
            // future cases...    
        }
    }

}

public enum SceneLoaderRequestType
{
    LOAD_SINGLE,
    UNLOAD_SINGLE,
    RELOAD_SINGLE,
    UNLOAD_CURRENT_GAME_LEVEL,
    UNLOAD_MAIN_MENU,
    UNLOAD_TYPE_UI
}