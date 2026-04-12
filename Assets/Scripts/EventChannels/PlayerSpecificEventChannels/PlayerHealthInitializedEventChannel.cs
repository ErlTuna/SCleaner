using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Player Events/Player Health Init Event Channel")]
public class PlayerHealthInitializedEventChannel : ScriptableObject
{
    public Action<UnitHealthData> OnEventRaised;

    public void RaiseEvent(UnitHealthData playerHealthData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(playerHealthData);
    }
}
