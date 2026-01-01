using UnityEngine;

public class WeaponPickup : ItemPickup<WeaponPickupDefinitionSO>
{
    [SerializeField] WeaponRuntimeData _weaponRuntimeData;

    void Start()
    {
        _weaponRuntimeData = null;
        // If hand-placed, create fresh runtime data
        if (_weaponRuntimeData == null && pickupDefinition.WeaponConfig != null)
        {
            _weaponRuntimeData = new WeaponRuntimeData(pickupDefinition.WeaponConfig);
        }

        SetupVisuals();
        UpdateColliderSize();
    }

    public override void OnPickupAttempt(GameObject collector)
    {
        if (CanBePickedUp(collector))
            OnCollected(collector);

        else
            Debug.Log("Can't pick up...");
    }

    public override bool CanBePickedUp(GameObject collector)
    {
        if (TryGetInventory(collector, out PlayerInventoryManager inventory))
        {
            if (inventory.CanPickupWeapon(pickupDefinition.WeaponConfig))
                return true;
        }

        return false;
    }

    public override void OnCollected(GameObject collector)
    {
        var inventory = collector.GetComponentInParent<PlayerInventoryManager>();
        if (inventory == null) return;

        
        inventory.TryAddWeapon(_weaponRuntimeData);
        Destroy(gameObject);
    }

    public void Initialize(WeaponPickupDefinitionSO definition, WeaponRuntimeData runtimeData)
    {
        if (runtimeData == null || runtimeData.Config == null || definition == null)
        {
            Debug.LogError("Weapon Runtime Data or Config or Pickup Definition is null. Can't create pickup");
            Destroy(gameObject);
            return;
        }

        pickupDefinition = definition;
        SetupVisuals();
    }

    void SetupVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = normalMaterial;
            spriteRenderer.sprite = pickupDefinition.PickupSprite;
        }
            
    }

}


