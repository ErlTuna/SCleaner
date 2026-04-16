using System;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Int Event Channel")]
public class IntEventChannelSO : ScriptableObject
{
    public Action<int> OnEventRaised;

    public void RaiseEvent(int value)
    {
        OnEventRaised?.Invoke(value);
    }
}
