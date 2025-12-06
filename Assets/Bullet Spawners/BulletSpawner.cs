using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour, IFactionMember
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] BulletSpawnPatternSO _spawnPattern;
    [SerializeField] ObjectPool<Bullet> _bulletPool;
    [SerializeField] int _defaultPoolCapacity = 20;
    [SerializeField] int _maxPoolSize = 30;
    [SerializeField] private Faction faction;
    public Faction Faction => faction;

    void Awake()
    {
        InitializePool();
    }

    void OnEnable()
    {
        StartCoroutine(_spawnPattern.Execute(this, gameObject));
    }
    
    public void FirePattern()
    {
        StartCoroutine(_spawnPattern.Execute(this, gameObject));
    }

    void InitializePool()
    {
        _bulletPool = new ObjectPool<Bullet>(
        createFunc: CreatePooledObject,
        actionOnGet: OnGetFromPool,
        actionOnRelease: OnReleaseToPool,
        actionOnDestroy: OnDestroyPooledObject,
        collectionCheck: false,
        defaultCapacity: _defaultPoolCapacity,
        maxSize: _maxPoolSize
        );
    }

    void EnsurePoolInitialized()
    {
        if (_bulletPool == null)
            InitializePool();
    }


    Bullet CreatePooledObject()
    {
        GameObject go = Instantiate(_bulletPrefab);
        go.SetActive(false);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.SetReturnAction(OnReturnToPool);
        return bullet;
    }

    void OnGetFromPool(Bullet bullet)
    {
        if (bullet != null)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(null);
        }

    }

    void OnReturnToPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    void OnReleaseToPool(Bullet bullet)
    {
        bullet.transform.SetParent(transform);
        bullet.transform.SetLocalPositionAndRotation(transform.position, Quaternion.identity);
        bullet.ResetBullet();
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public Bullet GetBullet()
    {
        EnsurePoolInitialized();
        return _bulletPool.Get();
    }


}
