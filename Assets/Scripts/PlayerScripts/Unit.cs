using UnityEngine;

public abstract class Unit : MonoBehaviour, IFactionMember
{
    [Header("Misc")]
    [SerializeField] Faction _faction;
    public Faction Faction => _faction;

    [Header("Event Channels")]
    [SerializeField] protected VoidEventChannelSO _defeatEventChannel;
    
    public abstract UnitStateData GetStateData();
}