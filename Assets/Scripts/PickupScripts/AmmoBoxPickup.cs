using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxPickUp : MonoBehaviour, IPickup
{
    public PickupSO pickupSO;
    private AudioSource audioSource;
    WeaponData weaponData;

    void Start(){
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

   void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            weaponData = other.GetComponentInChildren<PlayerWeaponManager>().currentWeaponScript.data;

            if(weaponData == null || weaponData.currentReserveAmmo == weaponData.maxReserveAmmo){
                PlayPickUpFailSound();
                return;
            }

            PickupEffect();
            
        }
   }

   public void PickupEffect(){

        if(weaponData.currentReserveAmmo + pickupSO.value > weaponData.maxReserveAmmo)
            weaponData.currentReserveAmmo = weaponData.maxReserveAmmo;

        else 
            weaponData.currentReserveAmmo += pickupSO.value;

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
   
}

