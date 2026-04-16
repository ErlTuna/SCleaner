using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/Loot Config")]
public class LootConfigSO : ScriptableObject
{
    public List<LootEntry> LootTable;
    public bool CanRollMultiCurrencyType;
    public int CurrencyRollCount;
    
}
