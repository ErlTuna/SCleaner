using System.Collections;
using UnityEngine;

public class SpriteBlink : MonoBehaviour
{
    Coroutine _blinkCoroutine;
    WaitForSeconds _blinkInterval = new(0.25f);
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] int _maxBlinkCount = 4;

    void OnEnable()
    {
        PlayerHealthManager.OnPlayerHitSpriteUpdate += StartSpriteBlink;
    }

    void OnDisable()
    {
        PlayerHealthManager.OnPlayerHitSpriteUpdate -= StartSpriteBlink;
    }

    void StartSpriteBlink() {
        _blinkCoroutine = StartCoroutine(Blink());
    }
    
    IEnumerator Blink(){
        int currentBlinkCount = 0;
        if (_spriteRenderer == null)
        {
            Debug.Log("Sprite Renderer is missing!");
            yield break;
        }
        while (currentBlinkCount < _maxBlinkCount)
        {
            _spriteRenderer.enabled = false;
            yield return _blinkInterval;
            _spriteRenderer.enabled = true;
            ++currentBlinkCount;
            yield return _blinkInterval;
        }
    }

}
