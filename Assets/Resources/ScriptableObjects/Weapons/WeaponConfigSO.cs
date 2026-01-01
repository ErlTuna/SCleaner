using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

[CreateAssetMenu(fileName = "WeaponConfigSO", menuName = "ScriptableObjects/Weapon/Weapon Config")]
public class WeaponConfigSO : ScriptableObject
{
    [Header("Inventory Item Data")]
    public InventoryItemDefinitionSO InventoryItem;

    [Header("Pickup Definition")]
    public WeaponPickupDefinitionSO PickupDefinition;

    // ItemID is a shortcut to InventoryItem.ItemID
    public string InventoryItemID => InventoryItem.ItemID;

    [Header("General Info")]
    public string WeaponName;
    public WeaponType Type;
    public GameObject Prefab;
    public GameObject BulletPrefab;
    public BulletConfigSO BulletConfig;
    public int Damage;
    public int RoundCapacity;
    public int MaxReserveAmmo;
    public int StartingReserveAmmo => MaxReserveAmmo;
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
    PRE_PRIMARY_ATTACK,
    PRIMARY_ATTACK,
    PRE_SECONDARY_ATTACK,
    SECONDARY_ATTACK,
    TRIGGER_RELEASED,
    RELOADING,
    DRY_FIRING
}
