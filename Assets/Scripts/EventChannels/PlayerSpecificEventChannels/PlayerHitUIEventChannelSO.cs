using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Player Events/Player Hit Event Channel")]
public class PlayerHitUIEventChannelSO : ScriptableObject
{
    public Action<int,int> OnEventRaised;

    public void RaiseEvent(int currentHealth, int maxHealth)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(currentHealth, maxHealth);
    }
}
