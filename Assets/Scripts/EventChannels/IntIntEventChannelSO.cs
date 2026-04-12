using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Int Int Event Channel")]
public class IntIntEventChannelSO : ScriptableObject
{
    public Action<int, int> OnEventRaised;

    public void RaiseEvent(int value1, int value2)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value1, value2);
    }
}
