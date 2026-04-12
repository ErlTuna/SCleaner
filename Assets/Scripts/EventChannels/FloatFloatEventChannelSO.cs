using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Float-Float Event Channel")]
public class FloatFloatEventChannel : ScriptableObject
{
    public Action<float, float> OnEventRaised;

    public void RaiseEvent(float value1, float value2)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value1, value2);
    }
}
