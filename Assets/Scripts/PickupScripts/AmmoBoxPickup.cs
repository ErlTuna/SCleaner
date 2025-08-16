using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxPickUp : MonoBehaviour, IPickup
{
    public PickupSO pickupSO;
    private AudioSource audioSource;
    WeaponSO _weaponInfo;

    void Start(){
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

   void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            _weaponInfo = other.GetComponentInChildren<WeaponManager>().currentWeaponScript.WeaponInfo;

            if(_weaponInfo == null || _weaponInfo.currentReserveAmmo == _weaponInfo.maxReserveAmmo){
                PlayPickUpFailSound();
                return;
            }

            PickupEffect();
            
        }
   }

   public void PickupEffect(){

        if(_weaponInfo.currentReserveAmmo + pickupSO.value > _weaponInfo.maxReserveAmmo)
            _weaponInfo.currentReserveAmmo = _weaponInfo.maxReserveAmmo;

        else 
            _weaponInfo.currentReserveAmmo += pickupSO.value;

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

