using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Player Events/Player Shield Health Changed Event Channel")]
public class PlayerShieldHealthChangedEventChannel : ScriptableObject
{
    public Action<StatChangedArgs> OnEventRaised;
    
    public void RaiseEvent(StatChangedArgs statChangedArgs)
    {
        OnEventRaised?.Invoke(statChangedArgs);
    }
}
