using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


// This is attached to the container that houses the ITEMS.
// Do NOT attach this to the root. 
// Ex :
// Group #1 :
// -Header
// -ItemContainer
// --children of ItemContainer

// The script should be attached to "ItemContainer" within the group and not Group #1, which would be the root.

[ExecuteAlways]
public class WrapLayout : MonoBehaviour, ILayoutController, ILayoutElement
{
    [Header("Layout")]
    [SerializeField] float horizontalSpacing = 8f;
    [SerializeField] float verticalSpacing = 8f;
    [SerializeField] bool resizeHeight = true;
    [SerializeField] bool isRebuilding = false;

    [Header("Padding")]
    [SerializeField] float itemContainerPaddingLeft;
    [SerializeField] float itemContainerPaddingRight;
    [SerializeField] float itemContainerPaddingTop;
    [SerializeField] float itemContainerPaddingBottom;
    [SerializeField] float headerAndItemPadding;

    [Header("Layout Elements")]
    [SerializeField] RectTransform _headerRect;
    [SerializeField] RectTransform _itemContainerRect;
    [SerializeField] RectTransform _groupParentRect;

    readonly HashSet<RectTransform> _preparedChildren = new();

    float _calculatedHeight;

     // ILayoutElemet Properties
    public float minWidth => -1;
    public float minHeight => -1;

    public float preferredWidth
    {
        get
        {
            // The parent VLG doesn’t care about width if it stretches, 
            // but could return the group’s rect width if needed
            return _groupParentRect != null ? _groupParentRect.rect.width : -1;
        }
    }

    public float preferredHeight
    {
        get
        {
            if (resizeHeight)
                return _calculatedHeight; // height from Rebuild()
            else
            {
                // computed when requested
                float totalHeight = _headerRect.rect.height
                                    + _itemContainerRect.rect.height
                                    + itemContainerPaddingTop
                                    + itemContainerPaddingBottom
                                    + headerAndItemPadding;
                return totalHeight;
            }
        }
    }

    public float flexibleWidth => -1;  
    public float flexibleHeight => -1; 
    public int layoutPriority => 1;     


    #if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying && isRebuilding == false)
            Rebuild();
    }
    #endif

    public void SetLayoutHorizontal() => Rebuild();
    public void SetLayoutVertical()   => Rebuild();

    public void Rebuild()
    {

        if (isRebuilding == true) return;
        //Debug.Log("Rebuilding layout...");
        isRebuilding = true;

        // Calculate maximum available width we can place items in.
        float usableWidth = _itemContainerRect.rect.width - itemContainerPaddingLeft - itemContainerPaddingRight;

        // Account for padding between header and item container.
        if (_headerRect != null) 
        {
            float yOffset = _headerRect.rect.height + headerAndItemPadding;
            _itemContainerRect.anchoredPosition = new Vector2(_itemContainerRect.anchoredPosition.x, -yOffset);
        }

        // Item starting position.
        float currentX = itemContainerPaddingLeft;
        float currentY = -itemContainerPaddingTop;

        // At minimum, we'll need to account for the combined height of our paddings on the y-axis.
        float usedHeight = itemContainerPaddingTop + itemContainerPaddingBottom;
        float currentRowHeight = 0f;

        List<InventoryEntry> entries = new();

        foreach (RectTransform child in GetContainerChildren())
        {
            // Skip inactive children.
            if (!child.gameObject.activeSelf)
                continue;

            // Compare preferred with if the child has a layout element.
            float childWidth = Mathf.Min(GetPreferredWidth(child), usableWidth);

            // Use preferred height or the rect's height.
            float childHeight = GetPreferredHeight(child);

            // If the element we'll add goes over the row's width, move to the next row.
            if ((currentX + childWidth) > (usableWidth + itemContainerPaddingLeft) && currentX > itemContainerPaddingLeft)
            {
                currentX = itemContainerPaddingLeft;
                currentY -= currentRowHeight + verticalSpacing;
                usedHeight += currentRowHeight + verticalSpacing;
                currentRowHeight = 0f;
            }

            // Set anchor, pivot to top left corner for consistency.
            child.anchorMin = child.anchorMax = new Vector2(0, 1);
            child.pivot = new Vector2(0, 1);
            child.anchoredPosition = new Vector2(currentX, currentY);

            // Account for horizontal spacing between elements for the next element to be drawn.
            currentX += childWidth + horizontalSpacing;

            // Account for the element's height. If the element is taller than the currentRowHeight, use that as the row height.
            currentRowHeight = Mathf.Max(currentRowHeight, childHeight);

            if (child.TryGetComponent(out InventoryEntry entry))
            {
                entries.Add(entry);
                entry.Up = entry.Down = entry.Left = entry.Right = null;
            }
        }

        // Account for final row
        usedHeight += currentRowHeight;

        // If chosen, expand the item container vertically. The anchors need to be set properly (top left corner) for this to work.
        if (resizeHeight)
        {
            _itemContainerRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                usedHeight
            );
        }
        
        // Expand the group's root rect transform after child calculations are complete 
        // header height + itemContainer height

        float newGroupHeight = _itemContainerRect.rect.height;
        _calculatedHeight = newGroupHeight;
        if (_headerRect != null) newGroupHeight += _headerRect.rect.height;
        
        _groupParentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newGroupHeight);


        // Navigation neighbor assignment
        AssignDirectionalNeighbors(entries);

        isRebuilding = false;

        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);

    }

    void AssignDirectionalNeighbors(List<InventoryEntry> entries)
    {
        float epsilon = 2f; // small offset to ensure we’re inside the next rect
        float stepSize = 5f; // distance per step along X or Y

        // Caching rects in a dictionary for quick bounds checking
        Dictionary<InventoryEntry, Rect> entryRects = new();
        foreach (InventoryEntry entry in entries)
        {
            RectTransform rt = entry.RectTransform;
            // Local space representation of entry's RectTransform
            Rect rect = new(rt.anchoredPosition.x, rt.anchoredPosition.y - rt.rect.height, rt.rect.width, rt.rect.height);
            entryRects[entry] = rect;
        }

        foreach (InventoryEntry entry in entries)
        {
            Rect rectA = entryRects[entry];

            // ----------------------
            // RIGHT neighbor
            // ----------------------
            float xCheck = rectA.xMax + horizontalSpacing + epsilon;
            float yCheck = rectA.center.y;
            InventoryEntry rightNeighbor = null;

            while (xCheck < _itemContainerRect.rect.width)
            {
                foreach (var other in entries)
                {
                    if (other == entry) continue;
                    Rect rectB = entryRects[other];

                    if (rectB.Contains(new Vector2(xCheck, yCheck)))
                    {
                        rightNeighbor = other;
                        break;
                    }
                }
                if (rightNeighbor != null) break;
                xCheck += stepSize;
            }
            entry.Right = rightNeighbor;

            // ----------------------
            // DOWN neighbor
            // ----------------------
            float yDownCheck = rectA.yMin - verticalSpacing - epsilon;
            float xDownCheck = rectA.center.x;
            InventoryEntry downNeighbor = null;

            while (yDownCheck > -_itemContainerRect.rect.height)
            {
                foreach (var other in entries)
                {
                    if (other == entry) continue;
                    Rect rectB = entryRects[other];

                    if (rectB.Contains(new Vector2(xDownCheck, yDownCheck)))
                    {
                        downNeighbor = other;
                        break;
                    }
                }
                if (downNeighbor != null) break;
                yDownCheck -= stepSize;
            }
            entry.Down = downNeighbor;

            // ----------------------
            // LEFT neighbor (mirror of right)
            // ----------------------
            float xLeftCheck = rectA.xMin - horizontalSpacing - epsilon;
            float yLeftCheck = rectA.center.y;
            InventoryEntry leftNeighbor = null;

            while (xLeftCheck > 0)
            {
                foreach (var other in entries)
                {
                    if (other == entry) continue;
                    Rect rectB = entryRects[other];

                    if (rectB.Contains(new Vector2(xLeftCheck, yLeftCheck)))
                    {
                        leftNeighbor = other;
                        break;
                    }
                }
                if (leftNeighbor != null) break;
                xLeftCheck -= stepSize;
            }
            entry.Left = leftNeighbor;

            // ----------------------
            // UP neighbor (mirror of down)
            // ----------------------
            float yUpCheck = rectA.yMax + verticalSpacing + epsilon;
            float xUpCheck = rectA.center.x;
            InventoryEntry upNeighbor = null;

            while (yUpCheck < epsilon + _itemContainerRect.rect.height)
            {
                foreach (var other in entries)
                {
                    if (other == entry) continue;
                    Rect rectB = entryRects[other];

                    if (rectB.Contains(new Vector2(xUpCheck, yUpCheck)))
                    {
                        upNeighbor = other;
                        break;
                    }
                }
                if (upNeighbor != null) break;
                yUpCheck += stepSize;
            }
            entry.Up = upNeighbor;
        }
    }


    // Helper method to get the center of a RectTransform in local space
    Vector2 GetRectCenter(RectTransform rect)
    {
        return rect.anchoredPosition + new Vector2(rect.rect.width / 2f, -rect.rect.height / 2f);
    }

    void PrepareChild(RectTransform child)
    {
        if (_preparedChildren.Contains(child))
            return;

        child.anchorMin = child.anchorMax = new Vector2(0, 1);
        child.pivot = new Vector2(0, 1);

        _preparedChildren.Add(child);
    }

    void AddItem(RectTransform item)
    {
        item.SetParent(_itemContainerRect, false);
        item.anchorMin = item.anchorMax = new Vector2(0, 1);
        item.pivot = new Vector2(0, 1);
        Rebuild();
    }

    float GetPreferredWidth(RectTransform rect)
    {
        if (rect.TryGetComponent(out LayoutElement layoutElement) && layoutElement.preferredWidth > 0)
            return layoutElement.preferredWidth;

        return rect.rect.width;
    }

    float GetPreferredHeight(RectTransform rect)
    {
        if (rect.TryGetComponent(out LayoutElement layoutElement) && layoutElement.preferredHeight > 0)
            return layoutElement.preferredHeight;
    
        return rect.rect.height;
    }

    IEnumerable<RectTransform> GetContainerChildren()
    {
        for (int i = 0; i < _itemContainerRect.childCount; i++)
            yield return _itemContainerRect.GetChild(i) as RectTransform;
    }

    public void CalculateLayoutInputHorizontal()
    {
        
    }

    public void CalculateLayoutInputVertical()
    {
        
    }
}
