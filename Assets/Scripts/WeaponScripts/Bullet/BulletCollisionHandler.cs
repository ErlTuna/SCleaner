using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    Bullet _owner;
    [SerializeField] Faction _ownerFaction;
    [SerializeField] Faction _targetFaction;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] Collider2D _collider2D;
    [SerializeField] GameObject _dissolveVFX;
    BulletData _bulletData;
    BulletConfigSO _bulletConfig;
    [SerializeField] LayerMask _blockingLayers;
    

    public void InitializeCollisionHandler(Bullet owner, BulletData bulletData, BulletConfigSO bulletConfig)
    {
        _owner = owner;
        _bulletData = bulletData;
        _bulletConfig = bulletConfig;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_bulletData.HasHitSomething) return;

        if (col.TryGetComponent(out IDamageable damageable))
        {
            // weird?..
            if (col != null)
            {
                float finalPushForce = CalculateFinalPushForce(col);
                DamageContext context = new(gameObject, _rb2D.velocity.normalized, transform.position, _bulletData.Damage, finalPushForce);
                damageable.TakeDamage(context);
            }
        }

        if ((_blockingLayers.value & (1 << col.gameObject.layer)) != 0)
            OnCollision();
    }

    void OnCollision()
    {
        _collider2D.enabled = false;
        _bulletData.HasHitSomething = true;
        Instantiate(_dissolveVFX, transform.position, Quaternion.identity);
        Cleanup();
    }

    void Cleanup()
    {
        if (_owner != null && _owner.ReturnToPoolCallback != null)
        {
            Debug.Log("Returning to pool: " + gameObject.name);
            _owner.ReturnToPool();
        }
        else
        {
            Debug.Log("No callback, destroying bullet: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    float CalculateFinalPushForce(Collider2D col)
    {
        float distance = Vector2.Distance(_bulletData.Owner.transform.position, col.transform.position);
        float safeDistance = Mathf.Max(distance, 0.01f); // in case distance is ever zero
        float finalPushForce = _bulletData.PushForce / safeDistance;

        if (finalPushForce > _bulletData.PushForce)
            finalPushForce = _bulletData.PushForce;

        return finalPushForce;
    }
}
