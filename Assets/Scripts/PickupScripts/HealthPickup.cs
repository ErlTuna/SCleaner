using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIX NEEDED!
// Must inherit IPickup inteface later

public class HealthPickup : MonoBehaviour
{
    /*public PickupSO pickupSO;
    public AudioSource audioSource;
    UnitInfo _playerHealth;

    void Start(){
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

   void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerHitbox")){
            // FIX THIS!!!
            //_playerHealth = other.GetComponent<IHealth>().UnitInfo;

            if (_playerHealth == null || _playerHealth.health == _playerHealth.maxHealth)
            {
                PlayPickUpFailSound();
                return;
            } 
            
            PickupEffect();
        }
   }

   public void PickupEffect(){

        if(_playerHealth.health + pickupSO.value > _playerHealth.maxHealth)
            _playerHealth.health = _playerHealth.maxHealth;

        else 
            _playerHealth.health += pickupSO.value;

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
   }*/


}
