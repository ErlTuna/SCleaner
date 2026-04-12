using System;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnEffectManager : MonoBehaviour
{   
    // Can be made an event channel later, perhaps...
    public static SpawnEffectManager Instance {get;set;}
    [SerializeField] GameObject _spawnVFX;
    [SerializeField] ObjectPool<PooledVFX> _pool;
    [SerializeField] int _defaultPoolCapacity = 10;
    [SerializeField] int _maxPoolSize = 10;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializePool();
    }

    void InitializePool()
    {
        _pool = new ObjectPool<PooledVFX>
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

    PooledVFX CreatePooledObject()
    {
        GameObject go = Instantiate(_spawnVFX);
        PooledVFX pooledVFX = go.GetComponent<PooledVFX>();
        pooledVFX.SetReturnAction(OnReleaseToPool);
        return pooledVFX;
    }

    void OnGetFromPool(PooledVFX pooledVFX)
    {
        if (pooledVFX != null)
        {
            pooledVFX.gameObject.SetActive(true);
            pooledVFX.transform.SetParent(null);
            pooledVFX.Play();
        }

    }

    void OnReturnToPool(PooledVFX pooledVFX)
    {
        _pool.Release(pooledVFX);
    }

    void OnReleaseToPool(PooledVFX pooledVFX)
    {
        Debug.Log("After image released to pool.");
        pooledVFX.transform.SetParent(transform);
        pooledVFX.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(PooledVFX pooledVFX)
    {
        Destroy(pooledVFX.gameObject);
    }

    public void PlaySpawnVFX(Vector3 location, Action onFinished)
    {
        PooledVFX vfx = _pool.Get();
        vfx.transform.position = location;
        vfx.Play(onFinished);
    }

}
