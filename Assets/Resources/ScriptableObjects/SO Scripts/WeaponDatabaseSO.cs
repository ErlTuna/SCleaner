using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tables/Weapon Table")]
public class WeaponDatabaseSO : ScriptableObject
{
    public List<PlayerWeaponConfigSO> Weapons => weapons;
    [SerializeField] List<PlayerWeaponConfigSO> weapons;

    Dictionary<string, PlayerWeaponConfigSO> _lookup;

    private bool _isInitialized = false;

    void Initialize()
    {
        if (_isInitialized) return;

        _lookup = new Dictionary<string, PlayerWeaponConfigSO>();

        foreach (var weapon in weapons)
        {
            if (weapon == null)
                continue;

            string id = weapon.InventoryItemGUID;

            if (_lookup.ContainsKey(id))
            {
                Debug.LogWarning($"Duplicate weapon ID detected: {id}");
                continue;
            }

            _lookup.Add(id, weapon);
        }

        _isInitialized = true;
    }

    public PlayerWeaponConfigSO Get(string id)
    {
        Initialize();

        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("WeaponDatabase Get called with null/empty ID");
            return null;
        }

        _lookup.TryGetValue(id, out var weapon);
        return weapon;
    }
}
