using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImage : MonoBehaviour
{
    Action<AfterImage> _returnToPoolCallback;
    [SerializeField] SpriteRenderer _spriteRenderer;
    AfterImageData _afterImageData;
    float _lifetime;
    float _timer;
    bool isActive = false;

    public void Setup( AfterImageData data, Vector3 worldScale)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        _afterImageData = data;
        _spriteRenderer.sprite = _afterImageData.AfterImageSprite;

        _spriteRenderer.material = _afterImageData.Material != null ? _afterImageData.Material : _spriteRenderer.material;
        transform.localScale = worldScale;

        _lifetime = _afterImageData.Lifetime;
        _timer = 0f;
        isActive = true;
    }

    public void SetReturnAction(Action<AfterImage> onReturn)
    {
        _returnToPoolCallback = onReturn;
    }

    void Update()
    {
        if (!isActive) return;

        _timer += Time.deltaTime;
        float t = Mathf.Clamp01(_timer / _lifetime);

        
        float alpha = Mathf.Lerp(_afterImageData.StartingAlpha, 0f, t);
        Color tintColor = _afterImageData.TintGradient.Evaluate(t);
        tintColor.a = alpha;

        _spriteRenderer.color = tintColor;

        if (_timer >= _lifetime)
        {
            isActive = false;
            _returnToPoolCallback?.Invoke(this);
        }
    }
}
