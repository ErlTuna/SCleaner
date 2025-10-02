using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/Weapon/Weapon Data")]
public class WeaponConfigSO : ScriptableObject
{
    [Header("General Info")]
    public string WeaponName;
    public WeaponType Type;
    public GameObject Prefab;
    public GameObject PickupPrefab;
    public BulletConfigSO BulletData;
    public string Name;
    public int Damage;
    public int RoundCapacity;
    public int MaxReserveAmmo;
    public int StartingReserveAmmo;
    public int BulletPerShot = 1;
    public float PushForce = 1f;

    [Header("Audio & VFX Info")]
    public Sprite Sprite;
    public AudioClip DryFireSFX;
    public AudioClip FiringSFX;
    public AudioClip WeaponSwitchSFX;
    

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
    RELOADING,
    DRY_FIRING
}
