using System;
using UnityEngine;


// This channel is for events that do not need specific data (have no arguments)

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    public Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
