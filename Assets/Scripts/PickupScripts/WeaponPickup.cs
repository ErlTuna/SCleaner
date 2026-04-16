using UnityEngine;

// Maybe I should have put the weapon runtime data inside the inventory of the player and pulled it from there...
public class WeaponPickup : ItemPickup, IPayloadProvider
{
    WeaponRuntime _weaponRuntimeData; // runtime

    [SerializeField] PlayerWeaponConfigSO _weaponConfig;
    
    
    public override void SetupVisuals()
    {
        if (visualsSR == null) return;

        if (visualsSR.sprite != null && autoUpdateSprite == false)
        {
            //_outline2D.Init();
            return;
        }

        if (itemConfig != null)
        {
            if (itemConfig.PickupDefinition != null)
            {
                if (itemConfig.PickupDefinition.DefaultPickupSprite != null)
                {
                    visualsSR.sprite = itemConfig.PickupDefinition.DefaultPickupSprite;
                    //_outline2D.Init();
                }
            }
        }       
    }


    public override void Highlight(bool higlighted)
    {
        if(visualsSR)
            visualsSR.sharedMaterial = higlighted ? outlineMaterial : normalMaterial;
        
    }

    public void InitializeWithExistingWeapon(WeaponRuntime droppedWeaponData)
    {
        _weaponRuntimeData = droppedWeaponData;

        _weaponConfig =  _weaponRuntimeData.Config as PlayerWeaponConfigSO;
        itemConfig = _weaponConfig.PickupConfig;

        SetupVisuals();
    }


    public IPickupPayload CreatePayload()
    {
        if (_weaponRuntimeData != null)
            return new WeaponPickupPayload(_weaponRuntimeData);

        WeaponRuntime newData = WeaponRuntimeFactory.Create(_weaponConfig);
        return new WeaponPickupPayload(newData);
    }

    public override void SetPickupConfig(ItemSO pickupConfig)
    {
        this.itemConfig = pickupConfig;
        WeaponPickupSO weaponPickupSO = pickupConfig as WeaponPickupSO;
        _weaponConfig = weaponPickupSO.WeaponConfig;
    }

}
