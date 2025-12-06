using System;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Scene Request Event Channel")]
public class SceneRequestEventChannelSO : ScriptableObject
{
    public Action<SceneLoaderRequest> OnEventRaised;

    public void RaiseEvent(SceneLoaderRequest request)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(request);
    }
}


