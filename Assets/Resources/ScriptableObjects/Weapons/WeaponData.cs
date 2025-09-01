using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("General Info")]
    public WeaponState state;
    public WeaponType type;
    public GameObject prefab;
    public BulletSO bulletData;
    public string weaponName;
    public int damage;
    public int currentAmmo;
    public int roundCapacity;
    public int maxReserveAmmo;
    public int currentReserveAmmo;
    public int pelletCount;

    [Header("Audio & VFX Info")]
    public AudioClip gunSound;
    public Sprite sprite;

    [Header("Misc")]
    public Vector3 offset = new(0, 0, 0);
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    
}

public enum WeaponType
{
    MELEE,
    RANGED
}

public enum WeaponState
{
    IDLE,
    FIRING,
    RELOADING
}
