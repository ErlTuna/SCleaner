using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelLoadedEventChannel _levelLoadedChannel;
    [SerializeField] Tilemap[] tilemaps; 

    void Start()
    {
        WorldBounds worldBounds = CalculateBounds();
        _levelLoadedChannel.RaiseEvent(worldBounds); 
    }


    WorldBounds CalculateBounds()
    {
        Vector2 min = new(float.MaxValue, float.MaxValue);
        Vector2 max = new(float.MinValue, float.MinValue);

        foreach (Tilemap tilemap in tilemaps)
        {
            BoundsInt bounds = tilemap.cellBounds;
            Vector3 bottomLeft = tilemap.CellToWorld(bounds.min);
            Vector3 topRight = tilemap.CellToWorld(bounds.max);
            min = Vector2.Min(min, bottomLeft);
            max = Vector2.Max(max, topRight);
        }

        return new WorldBounds(min, max);
    }
}


public readonly struct WorldBounds
{
    public readonly Vector2 Min;
    public readonly Vector2 Max;

    public WorldBounds(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }
}
