using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Player Events/Player Health Changed Event Channel")]
public class PlayerHealthChangedEventChannel : ScriptableObject
{
    public Action<StatChangedArgs> OnEventRaised;
    
    public void RaiseEvent(StatChangedArgs statChangedArgs)
    {
        OnEventRaised?.Invoke(statChangedArgs);
    }
}
