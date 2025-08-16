using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/Weapon Info")]
public class WeaponSO : ScriptableObject
{
    public Sprite sprite;
    public AudioClip gunSound;
    public GameObject bulletPrefab;
    public WeaponType type;
    public BulletSO bulletInfo;
    public Vector3 offset = new(0, 0, 0);
    public string weaponName;
    public int damage;
    public int currentAmmo;
    public int roundCapacity;
    public int maxReserveAmmo;
    public int currentReserveAmmo;
    public int pelletCount;
    public bool isReloading;
    public bool isFiring;

    public void Init(){
        currentAmmo = roundCapacity;
        currentReserveAmmo = maxReserveAmmo;
        isReloading = false;
        isFiring = false;
    }

    public enum WeaponType{
    MELEE,
    RANGED
}

}


