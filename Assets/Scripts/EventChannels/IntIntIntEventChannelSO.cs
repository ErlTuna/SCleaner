using System;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Int Int Int Event Channel")]

public class IntIntIntEventChannelSO : ScriptableObject
{
    public Action<int, int, int> OnEventRaised;

    public void RaiseEvent(int value1, int value2, int value3)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value1, value2, value3);
    }
}
