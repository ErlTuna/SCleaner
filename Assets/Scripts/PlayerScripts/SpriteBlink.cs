using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlink : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    readonly int blinkCount = 4;

    void OnEnable(){
        PlayerHealth.onPlayerHitInvuln += Blink;
        
    }

    void OnDisable(){
        PlayerHealth.onPlayerHitInvuln -= Blink;
    }
    
    IEnumerator Blink(){
        int currentBlinkCount = 0;
        while (currentBlinkCount < blinkCount){
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.enabled = true;
            ++currentBlinkCount;
            yield return new WaitForSeconds(0.25f);
        }
    }

}
