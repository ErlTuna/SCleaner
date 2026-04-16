using UnityEngine;

public class BaseInteractionContext
{
    public PlayerMain Player { get; }
    public Vector3 Position { get; }
    public float PickupTime { get; }

    public BaseInteractionContext(PlayerMain player)
    {
        Player = player;
        Position = player.transform.position;
        PickupTime = Time.time;
    }

}
