using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Loot/Loot Item")]
public class LootItemSO : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
}
