using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxPickUp : MonoBehaviour, IPickup
{
    public PickupSO pickupSO;
    private AudioSource audioSource;
    WeaponConfigSO weaponData;

    public Vector3 Location { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start(){
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

   void OnTriggerEnter2D(Collider2D other){

        /*if (other.CompareTag("Player"))
        {
            weaponData = other.GetComponentInChildren<PlayerWeaponManager>().currentWeaponScript.WeaponConfig;

            if (weaponData == null || weaponData.StartingReserveAmmo == weaponData.MaxReserveAmmo)
            {
                PlayPickUpFailSound();
                return;
            }

            PickupEffect();

        }*/
   }

    public void OnCollected()
    {
    
    }

   public void PickupEffect()
    {

        if (weaponData.StartingReserveAmmo + pickupSO.value > weaponData.MaxReserveAmmo)
            weaponData.StartingReserveAmmo = weaponData.MaxReserveAmmo;

        else
            weaponData.StartingReserveAmmo += pickupSO.value;

        PlayPickupSuccess();
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, audioSource.clip.length);
    }

   public void PlayPickupSuccess(){
        if (audioSource != null){
            audioSource.clip = pickupSO.PickUpAudio;
            audioSource.PlayOneShot(pickupSO.PickUpAudio);
        }
   }

   public void PlayPickUpFailSound(){
    if (audioSource != null){
        audioSource.clip = pickupSO.PickUpAudio;
            audioSource.PlayOneShot(pickupSO.FailToPickUpAudio);
        }
   }


    public bool CanBePickedUp(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public void OnCollected(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public void OnPickupAttempt(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public void HighlightPickup(bool higlight)
    {
        throw new System.NotImplementedException();
    }

    /*
    public void ShowPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void HidePrompt()
    {
        throw new System.NotImplementedException();
    }
    */
}

