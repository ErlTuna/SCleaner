using System;
public static class SceneLoaderRequestExtensions
{
    /// <summary>
    /// Sets the request to show a loading screen.
    /// </summary>
    public static SceneLoaderRequest WithLoadingScreen(this SceneLoaderRequest req, bool show = true)
    {
        return new SceneLoaderRequest
        {
            SceneReference = req.SceneReference,
            RequestType = req.RequestType,
            SetSceneActive = req.SetSceneActive,
            ShowLoadingScreen = show,
            OnCompleted = req.OnCompleted
        };
    }

    /// <summary>
    /// Sets the request to mark the scene as active after loading.
    /// </summary>
    public static SceneLoaderRequest SetActive(this SceneLoaderRequest req, bool active = true)
    {
        return new SceneLoaderRequest
        {
            SceneReference = req.SceneReference,
            RequestType = req.RequestType,
            SetSceneActive = active,
            ShowLoadingScreen = req.ShowLoadingScreen,
            OnCompleted = req.OnCompleted
        };
    }

    /// <summary>
    /// Adds a callback to be invoked when the request completes.
    /// </summary>
    public static SceneLoaderRequest OnComplete(this SceneLoaderRequest req, Action callback)
    {
        return new SceneLoaderRequest
        {
            SceneReference = req.SceneReference,
            RequestType = req.RequestType,
            SetSceneActive = req.SetSceneActive,
            ShowLoadingScreen = req.ShowLoadingScreen,
            OnCompleted = callback
        };
    }

    /// <summary>
    /// Combines multiple optional flags and a callback for convenience.
    /// </summary>
}

