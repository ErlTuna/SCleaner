using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player SOs", menuName = "ScriptableObjects/Component SOs/Player Inventory")]
public class UnitInventoryConfigSO : ScriptableObject
{
    [Header("Currency")]
    public int currency = 0;
    public int maxCurrency = 9999;

    [Header("Weapons")]
    //public List<WeaponConfig> weaponConfigs;
    //public List<WeaponData> weaponConfigs;
    public List<GameObject> weaponPrefabs = new();

    public int weaponsHeld;

    void OnEnable()
    {
        weaponsHeld = weaponPrefabs.Count;
    }

}
