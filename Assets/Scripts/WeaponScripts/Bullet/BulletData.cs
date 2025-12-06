using System;
using UnityEngine;

[Serializable]
public class BulletData
{
    public GameObject Owner;
    public Faction Faction;
    public int Damage;
    public float Size;
    public float ProjectileSpeed;
    public float PushForce;
    public Vector2 Trajectory;
    public float LifeTime;
    public bool HasHitSomething;

    // this is for bullets that are not spawned by weapons
    public BulletData(GameObject _owner, BulletConfigSO config, Faction faction)
    {
        Owner = _owner;
        Damage = config.DefaultDamage;
        Size = config.DefaultSize;
        ProjectileSpeed = config.DefaultProjectileSpeed;
        PushForce = config.DefaultPushForce;
        LifeTime = config.DefaultLifeTime;
        Trajectory = _owner.transform.right;
        Faction = faction;
    }

    // this is for bullets that are spawned by weapons
    public BulletData(GameObject owner, BulletConfigSO config, WeaponRuntimeData weaponData, WeaponConfigSO weaponConfig, Faction faction)
    {
        Owner = owner;
        Damage = weaponData.Damage;
        Size = config.DefaultSize;
        ProjectileSpeed = config.DefaultProjectileSpeed;
        PushForce = weaponConfig.PushForce;
        LifeTime = config.DefaultLifeTime;
        Faction = faction;
    }
}
