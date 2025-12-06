using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[CreateAssetMenu(fileName = "WeaponConfigSO", menuName = "ScriptableObjects/Weapon/Weapon Config")]
public class WeaponConfigSO : ScriptableObject
{
    [Header("General Info")]
    public string WeaponName;
    public WeaponType Type;
    public GameObject Prefab;
    public GameObject PickupPrefab;
    public GameObject BulletPrefab;
    public BulletConfigSO BulletConfig;
    public string Name;
    public int Damage;
    public int RoundCapacity;
    public int MaxReserveAmmo;
    public int StartingReserveAmmo;
    public int BulletPerShot = 1;
    public float PushForce = 1f;
    public float SpreadAngle = 0f;
    public float SpreadResetThreshold = .1f;
    public bool HasInfiniteReserveAmmo = false;

    [Header("Audio & VFX Info")]
    public Sprite Sprite;
    public SoundData FiringSound;
    public SoundData DryFiringSound;
    public AudioClip DryFireSFX;
    public AudioClip FiringSFX;
    public AudioClip WeaponSwitchSFX;
    

    [Header("Misc")]
    public Sprite UI_Icon;
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
