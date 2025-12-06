using System;
using UnityEngine;

// Event channel for locating transforms of loaded objects (i.e : player)

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Transform Event Channel")]
public class TransformEventChannelSO : ScriptableObject
{
    public event Action<Transform> OnEventRaised;

    public void RaiseEvent(Transform t)
    {
        
        if (OnEventRaised != null)
        {
            OnEventRaised?.Invoke(t);
        }
        
    }
}
