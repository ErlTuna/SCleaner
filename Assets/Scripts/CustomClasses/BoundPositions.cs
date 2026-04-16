using UnityEngine;

public struct BoundPositions
{
    public Vector2 center;
    public readonly float left;
    public readonly float right;
    public readonly float top;
    public readonly float bottom;
    public BoundPositions(BoxCollider2D area)
    {
        center = area.bounds.center;
        left = center.x - area.bounds.extents.x;
        right = center.x + area.bounds.extents.x;
        top = center.y + area.bounds.extents.y;
        bottom = center.y - area.bounds.extents.y;
    }
}
