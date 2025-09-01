using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player SOs", menuName = "ScriptableObjects/Component SOs/Player Inventory")]
public class PlayerInventorySO : ScriptableObject
{
    [Header("Currency")]
    public int currency = 0;
    public int maxCurrency = 9999;

    [Header("Weapons")]
    public List<WeaponData> weaponDatas;
    //public List<GameObject> weaponPrefabs;

    public int weaponsHeld;

    void OnEnable()
    {
        //weaponPrefabs = new();
        weaponsHeld = weaponDatas.Count;
    }

}
