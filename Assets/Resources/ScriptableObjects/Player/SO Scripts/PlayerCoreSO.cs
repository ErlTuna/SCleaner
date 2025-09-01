using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player X", menuName = "ScriptableObjects/Unit Data/Player Data")]
public class PlayerCoreSO : ScriptableObject
{
    public HealthSO healthData;
    public MovementSO movementData;
    public UnitStateSO stateData;
    public PlayerInventorySO inventory;
    public PlayerSoundsSO sounds;
    public EnergySO energyData;



}
