using UnityEngine;

public abstract class ItemPickup<TDef> : MonoBehaviour, IPickup, IHighlightable
where TDef : PickupDefinitionSO
{
    
    [SerializeField] BoxCollider2D _boxCollider2D;
    [SerializeField] protected TDef pickupDefinition;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Material normalMaterial;
    [SerializeField] protected Material outlineMaterial;
    public Vector3 Location => transform.position;
    

    public abstract void OnPickupAttempt(GameObject collector);
    public abstract bool CanBePickedUp(GameObject collector);
    public abstract void OnCollected(GameObject collector);

    public void UpdateColliderSize()
    {
        if (spriteRenderer.sprite == null) return;


        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        _boxCollider2D.size = spriteSize;
        _boxCollider2D.offset = spriteRenderer.sprite.bounds.center;
    }

    public void Highlight(bool higlighted)
    {
        if(spriteRenderer != null)
            spriteRenderer.sharedMaterial = higlighted ? outlineMaterial : normalMaterial;
    }

    protected bool TryGetInventory(GameObject collector, out PlayerInventoryManager inventory)
    {
        return collector.TryGetComponent(out inventory);
    }

}

