using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanBePickedUp", menuName = "ScriptableObjects/Pickups/Pickup Conditions/Items/CanBePickedUp")]
public class PickupCondition_CanPickUpPassiveItem : PickupConditionSO
{
    public override bool Evaluate(PickupExecutionContext ctx)
    {
        if (ctx == null)
        {
            Debug.Log("Context is missing.");
            return false;
        }

        if (ctx.PickupTransactionData == null)
        {
            Debug.Log("Transaction data is missing.");
            return false;
        }

        if (ctx.PickupTransactionData.Payload == null)
        {
            Debug.Log("Payload is missing.");
            return false;
        }

        if (ctx.PickupTransactionData.Payload is not PassiveItemPickupPayload passiveItem)
            return false;

        if (!ctx.TryGet<PlayerInventoryManager>(out var inventory))
            return false;

        return inventory.CanAddPassiveItem(passiveItem.PassiveItemSO);
    }
}
