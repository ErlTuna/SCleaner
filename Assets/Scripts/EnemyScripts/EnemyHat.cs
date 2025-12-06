using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyHat : MonoBehaviour, IDamageable, IDetachable
{
    public Action OnHatKnockedDown;
    [SerializeField] HatConfig _config;
    [SerializeField] Rigidbody2D _rigidBody;
    [SerializeField] Collider2D _col2D;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _health;

    bool _isDetached = false;

    [SerializeField] float spinForce = 3f;

    void Start()
    {
        _health = _config.MaxHealth;
        _spriteRenderer.sprite = _config.DefaultVisual;
        //_rigidBody.isKinematic = true;
        //_col2D.isTrigger = true;
    }

    /*
    public void KnockOff(Vector2 force)
    {
        if (_isDetached) return;
        _isDetached = true;

        transform.parent = null;
        _rigidBody.isKinematic = false;
        col2D.isTrigger = false;

        _rigidBody.AddForce(force, ForceMode2D.Impulse);
        _rigidBody.AddTorque(Random.Range(-spinForce, spinForce), ForceMode2D.Impulse);
    }
    */

    public void TakeDamage(DamageContext context)
    {
        if (_isDetached) return;
        if (_rigidBody == null) return;
        if (_health <= 0) return;

        _health -= context.Damage;
        Debug.Log("Hat taking damage...");

        if (_health <= 0)
        {
            _health = 0;
            OnHatKnockedDown?.Invoke();
            Detach();
            Vector2 force = context.PushForce * context.HitterMovementVector.normalized;
            ApplyPushForce(force);
        }

    }
    public void Detach()
    {
        Debug.Log("Detaching hat");
        _isDetached = true;
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _col2D.isTrigger = false;
        int layerIndex = (int)Mathf.Log(_config.DetachedLayer.value, 2);
        gameObject.layer = layerIndex;
        _config.KnockDownBehaviour.Execute(gameObject);
    }

    void ApplyPushForce(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
        _rigidBody.AddTorque(Random.Range(-spinForce, spinForce), ForceMode2D.Impulse);
    }

    public void TakeDamage(int amount)
    {
        // no op
    }

    public void Detach(DamageContext damageContext)
    {
        Debug.Log("Detaching hat");
        _isDetached = true;
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _col2D.isTrigger = false;
        int layerIndex = (int)Mathf.Log(_config.DetachedLayer.value, 2);
        gameObject.layer = layerIndex;
        Vector2 force = damageContext.PushForce * damageContext.HitterMovementVector.normalized;
        ApplyPushForce(force);
        _config.KnockDownBehaviour.Execute(gameObject);
    }
}
