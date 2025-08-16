using System;
using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public delegate void OnPlayerHitDamagetEventHandler(int amount);
    public static OnPlayerHitDamagetEventHandler onPlayerHitTakeDamage;

    public delegate void OnPlayerHitAudioEventHandler();
    public static OnPlayerHitAudioEventHandler onPlayerHitAudio;

    void OnTriggerEnter2D(Collider2D other)
    {
        IEnemy attacker = other.GetComponent<IEnemy>();
        if (attacker != null){
            onPlayerHitTakeDamage?.Invoke(5);
        }
    }
}
