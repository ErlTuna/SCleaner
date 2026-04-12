using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoConfigSO", menuName = "ScriptableObjects/Weapon/Ammo Config")]
public class WeaponAmmoConfigSO : ScriptableObject, IAmmoConfig
{
    [SerializeField] int _roundCapacity;
    [SerializeField] int _maxReserveAmmo;
    [SerializeField] bool _hasInfiniteReserveAmmo;

    public int RoundCapacity => _roundCapacity;
    public int MaxReserveAmmo => _maxReserveAmmo;
    public bool HasInfiniteReserveAmmo => _hasInfiniteReserveAmmo;
}

