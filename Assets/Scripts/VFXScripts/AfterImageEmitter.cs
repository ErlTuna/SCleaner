using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageEmitter : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _afterImagePrefab;
    [SerializeField] AfterImageData _afterImageData;
    [SerializeField] ObjectPool<AfterImage> _pool;
    [SerializeField] int _defaultPoolCapacity = 10;
    [SerializeField] int _maxPoolSize = 10;
    float _lastSpawnTime;


    void Awake()
    {
        InitializePool();
    }

    void Start()
    {
        if (_spriteRenderer != null && _afterImageData != null)
            _afterImageData.AfterImageSprite = _spriteRenderer.sprite;
    }

    void InitializePool()
    {
        _pool = new ObjectPool<AfterImage>
        (
        createFunc: CreatePooledObject,
        actionOnGet: OnGetFromPool,
        actionOnRelease: OnReleaseToPool,
        actionOnDestroy: OnDestroyPooledObject,
        collectionCheck: false,
        defaultCapacity: _defaultPoolCapacity,
        maxSize: _maxPoolSize
        );
    }

    AfterImage CreatePooledObject()
    {
        GameObject go = Instantiate(_afterImagePrefab);
        AfterImage afterImage = go.GetComponent<AfterImage>();
        afterImage.SetReturnAction(OnReleaseToPool);
        return afterImage;
    }

    void OnGetFromPool(AfterImage afterImage)
    {
        if (afterImage != null)
        {
            afterImage.gameObject.SetActive(true);
            afterImage.transform.SetParent(null);
        }

    }

    void OnReturnToPool(AfterImage afterImage)
    {
        _pool.Release(afterImage);
    }

    void OnReleaseToPool(AfterImage afterImage)
    {
        afterImage.transform.SetParent(transform);
        afterImage.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(AfterImage ai)
    {
        Destroy(ai.gameObject);
    }

    public void TryEmit()
    {
        if (Time.time < _lastSpawnTime + _afterImageData.SpawnRate) return;

        AfterImage afterImage = _pool.Get();
        afterImage.transform.SetPositionAndRotation(transform.position, transform.rotation);
        afterImage.Setup(_afterImageData, _spriteRenderer.transform.lossyScale);
        afterImage.SetReturnAction(OnReturnToPool);
        _lastSpawnTime = Time.time;
    }


}
