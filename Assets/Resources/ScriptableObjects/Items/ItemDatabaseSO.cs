using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tables/Item Table")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemDatabaseEntry> DatabaseEntries;
}
