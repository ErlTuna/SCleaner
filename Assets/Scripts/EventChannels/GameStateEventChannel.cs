using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Game State Change Event Channel")]
public class GameStateEventChannel : ScriptableObject
{
    public Action<GameState> OnEventRaised;

    public void RaiseEvent(GameState newState)
    {
        OnEventRaised?.Invoke(newState);
    }
}
