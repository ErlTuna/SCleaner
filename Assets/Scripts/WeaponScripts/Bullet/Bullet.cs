using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public Action<Bullet> ReturnToPoolCallback { get; private set; }
    [SerializeField] Collider2D _collider2D;
    [SerializeField] BulletConfigSO _bulletConfig;
    [SerializeField] BulletData _bulletData;
    [SerializeField] BulletMover _mover;
    [SerializeField] BulletCollisionHandler _collisionHandler;
    [SerializeField] Faction _faction;
    [SerializeField] SpriteRenderer _visualsSr;

    void Update(){
        _bulletData.LifeTime -= Time.deltaTime;
        if (_bulletData.LifeTime <= 0){
            OnLifeTimeEnd();
        }
    }

    public void Initialize(GameObject weapon, Vector2 trajectory, WeaponRuntimeData weaponData, WeaponConfigSO weaponConfig, int sortingOrder = 0)
    {
        _bulletConfig = weaponConfig.BulletConfig;
        _bulletData = new(weapon, _bulletConfig, weaponData, weaponConfig, _faction)
        {
            Trajectory = trajectory,
            Faction = _faction
        };

        SetSortingOrder(sortingOrder);
        _collisionHandler.InitializeCollisionHandler(this, _bulletData, _bulletConfig);
        _mover.InitializeMover(trajectory, _bulletData.ProjectileSpeed);
    }

    public void Initialize(GameObject bulletSpawner, Vector2 trajectory, int sortingOrder = 0)
    {
        _bulletData = new(bulletSpawner, _bulletConfig, _faction)
        {
            Trajectory = trajectory,
            Faction = _faction
        };

        SetSortingOrder(sortingOrder);
        _collisionHandler.InitializeCollisionHandler(this, _bulletData, _bulletConfig);
        _mover.InitializeMover(trajectory, _bulletData.ProjectileSpeed);

    }

    void OnLifeTimeEnd()
    {
        if (ReturnToPoolCallback != null)
            ReturnToPoolCallback.Invoke(this);

        else 
            Destroy(gameObject);
    }

    public void ResetBullet()
    {
        _collider2D.enabled = true;
        _bulletData.HasHitSomething = false;
        _bulletData.LifeTime = _bulletConfig.DefaultLifeTime;
    }

    public void SetReturnAction(Action<Bullet> onReturn)
    {
        ReturnToPoolCallback = onReturn;
    }

    public void ReturnToPool()
    {
        ReturnToPoolCallback?.Invoke(this);
    }

    public void SetSortingOrder(int value)
    {
        if (_visualsSr)
            _visualsSr.sortingOrder = value;
    }


}
