using UnityEngine;

public class PassiveItemPickup : ItemPickup, IPayloadProvider
{
    void Start()
    {
        SetupVisuals();
                
        if (autoUpdateCollider)
            UpdateColliderSize();
    }

    /*
    public override void OnPickupAttempt(GameObject collector)
    {
        PlayerMain playerMain = collector.GetComponent<PlayerMain>();
        PassiveItemSO passiveItemSO = pickupConfig as PassiveItemSO;

        PickupTransactionData pickupTransactionData = new(pickupConfig, new PassiveItemPickupPayload(passiveItemSO));

        pickupExecutionContext = new(playerMain, pickupTransactionData);
        
        if (CanBePickedUp(pickupExecutionContext))
            OnCollected();
    }
    */


    public override void OnCollected()
    {
        foreach (PickupBehaviorEntry behaviorEntry in itemConfig.Behaviors)
        {
            if (ItemTracker.Instance.HasBeenConsumed(itemConfig.ItemID) && behaviorEntry.IsExhaustible)
                continue;
            

            behaviorEntry.TryApply(pickupExecutionContext);
        }

        // Notify UI / events
        if (_itemPickedUpNotificationEventChannel != null && TryGetComponent(out SpriteRenderer sr))
        {

            PickedUpItemData itemData = new()
            {
                name = itemConfig.PickupDefinition.PickUpName,
                description = itemConfig.PickupDefinition.PickUpDescription,
                icon = sr.sprite
            };

            _itemPickedUpNotificationEventChannel.RaiseEvent(itemData);
        }

        ItemTracker.Instance.MarkAsConsumed(itemConfig.ItemID);
        Destroy(gameObject);
    }

    public override void SetupVisuals()
    {
        // Priority order:
        // 1. Explicitly set by level designer
        // 2. Provided by wrapped item (weapon)
        // 3. Default from definition

        if (visualsSR.sprite != null)
            return;

        if (itemConfig != null && itemConfig.PickupDefinition.DefaultPickupSprite != null)
            visualsSR.sprite = itemConfig.PickupDefinition.DefaultPickupSprite;
        
    }

    protected override void UpdateColliderSize()
    {
        if (visualsSR.sprite == null) return;
        if (boxCollider2D == null) return;

        

        Vector2 spriteSize = visualsSR.sprite.bounds.size;
        boxCollider2D.size = spriteSize;
        boxCollider2D.offset = visualsSR.sprite.bounds.center;
    }

    public IPickupPayload CreatePayload()
    {
        throw new System.NotImplementedException();
    }
}
