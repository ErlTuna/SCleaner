using Unity.VisualScripting;
using UnityEngine;

public class AmmoBoxPickUp : ItemPickup<WeaponPickupDefinitionSO>
{
    
    [SerializeField] AudioSource audioSource;
    WeaponConfigSO weaponData;
    int value;

    public void OnCollected()
    {
    
    }

   public void PickupEffect()
    {
        /*
        if (weaponData.StartingReserveAmmo + value > weaponData.MaxReserveAmmo)
            weaponData.StartingReserveAmmo = weaponData.MaxReserveAmmo;

        else
            weaponData.StartingReserveAmmo += value;

        //PlayPickupSuccess();
        spriteRenderer.enabled = false;
        Destroy(gameObject, audioSource.clip.length);
        */
    }

    
    public override void OnPickupAttempt(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanBePickedUp(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollected(GameObject collector)
    {
        throw new System.NotImplementedException();
    }
    
}

