using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pickups/Passive Item")]
public class PassiveItemSO : ItemSO
{
    public override IPickupPayload CreatePayload()
    {
        return new PassiveItemPickupPayload(this);
    }
}

