using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Player Events/Player Max Health Changed Event Channel")]
public class PlayerMaxHealthChangedEventChannel : ScriptableObject
{
    public Action<StatChangedArgs> OnEventRaised;
    
    public void RaiseEvent(StatChangedArgs statChangedArgs)
    {
        OnEventRaised?.Invoke(statChangedArgs);
    }
}
