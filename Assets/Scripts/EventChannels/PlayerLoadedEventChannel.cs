using System;
using UnityEngine;


// This channel is for when the player loads in (i.e : a level is loaded)

public class PlayerLoadedEventChannel : MonoBehaviour
{
    public Action<Transform> OnEventRaised;

    public void RaiseEvent(Transform playerTransform)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(playerTransform);
    }
}
