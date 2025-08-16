using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color _flashColor = Color.white;
    [SerializeField] float _flashTime = 0.25f;

    public SpriteRenderer _spriteRenderer;
    Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        _mat = _spriteRenderer.material;
        SetFlashColor();
    }

    public void TriggerDamageFlash(){
        StartCoroutine(DamageFlasher());
    }

    IEnumerator DamageFlasher(){
        float currentFlashAmount;
        float elapsedTime = 0f;

        while (elapsedTime < _flashTime){
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime/_flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    void SetFlashAmount(float amount){
        _mat.SetFloat("_FlashAmount", amount);
    }

    void SetFlashColor(){
        _mat.SetColor("_FlashColor", _flashColor);
    }

}
