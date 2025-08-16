using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutAndBolt : MonoBehaviour, IPickup
{

    public PickupSO pickupSO;
    public AudioSource audioSource;
    UnitInfoSO playerInfo;
    void Start(){
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            playerInfo = other.GetComponent<PlayerMain>().playerInfo;
            
            if(playerInfo != null && (playerInfo.currency != playerInfo.maxCurrency)){
                PickupEffect();
                PlayPickupSuccess();
                GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, audioSource.clip.length);
            }
                
        }
   }
    public void PickupEffect()
    {
        //notify UI manager
        playerInfo.currency += pickupSO.value;
    }

    public void PlayPickUpFailSound()
    {
        //no op
    }

    public void PlayPickupSuccess()
    {
        if (audioSource != null){
            audioSource.clip = pickupSO.PickUpAudio;
            audioSource.PlayOneShot(pickupSO.PickUpAudio);
        }
    }

}
