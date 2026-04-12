using UnityEngine;

public class DummyPickup : ItemPickup
{
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

    /*
    public override void OnPickupAttempt(GameObject collector)
    {
        PlayerMain playerMain = collector.GetComponent<PlayerMain>();
        PassiveItemSO passiveItemSO = pickupConfig as PassiveItemSO;

        PickupTransactionData pickupTransactionData = new(pickupConfig, new PassiveItemPickupPayload(passiveItemSO));

        pickupExecutionContext = new(playerMain, pickupTransactionData);
        
        
        OnCollected();
    }
    */
}
