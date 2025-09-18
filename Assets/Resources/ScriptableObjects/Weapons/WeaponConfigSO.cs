using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/Weapon/Weapon Data")]
public class WeaponConfigSO : ScriptableObject
{
    [Header("General Info")]
    public WeaponState State;
    public WeaponType Type;
    public GameObject Prefab;
    public BulletSO BulletData;
    public string Name;
    public int Damage;
    public int CurrentAmmo;
    public int RoundCapacity;
    public int MaxReserveAmmo;
    public int StartingReserveAmmo;
    public int BulletPerShot;

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

    void OnEnable()
    {
        State = WeaponState.IDLE;
        CurrentAmmo = RoundCapacity;
        StartingReserveAmmo = MaxReserveAmmo;
    }

}

public enum WeaponType
{
    MELEE,
    RANGED
}

public enum WeaponState
{
    IDLE,
    PRE_PRIMARY_ATTACK,
    PRIMARY_ATTACK,
    PRE_SECONDARY_ATTACK,
    SECONDARY_ATTACK,
    TRIGGER_RELEASED,
    RELOADING
}
