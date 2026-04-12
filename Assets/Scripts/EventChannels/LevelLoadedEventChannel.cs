using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Level Loaded Event Channel")]
public class LevelLoadedEventChannel : ScriptableObject
{
    public event Action<WorldBounds> OnEventRaised;
    public void RaiseEvent(WorldBounds worldBounds)
    {
        
        if (OnEventRaised != null)
        {
            OnEventRaised?.Invoke(worldBounds);
        }
        
    }
}
