using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfigSO", menuName = "ScriptableObjects/Weapon/Weapon Config")]
public class WeaponConfigSO : ScriptableObject
{
    [Header("General Info")]
    public string WeaponName;
    public WeaponType Type;
    public GameObject Prefab;
    public GameObject BulletPrefab;
    public BulletConfigSO BulletConfig;
    public WeaponAmmoConfigSO AmmoConfig;
    public int Damage;
    public int BulletPerShot = 1;
    public float PushForce = 1f;
    public float SpreadAngle = 0f;
    public float SpreadResetThreshold = .1f;

    [Header("Flags")]
    public bool CanBeDropped = false;

    [Header("Audio & VFX Info")]
    public Sprite Sprite;
    public SoundDataSO FiringSoundData;
    public SoundDataSO DryFiringSoundData;
    

    [Header("Misc")]
    public Vector3 offset = new(0, 0, 0);
    

    /*
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    */

}

public enum WeaponType
{
    MELEE,
    RANGED
}

public enum WeaponState
{
    IDLE,
    CHARGING_PRIMARY_ATTACK,
    PRIMARY_ATTACK,
    PRE_SECONDARY_ATTACK,
    SECONDARY_ATTACK,
    TRIGGER_RELEASED,
    RELOADING,
    DRY_FIRING
}
