using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Config", menuName = "ScriptableObjects/Component Configs/Energy Config")]
public class PlayerEnergyConfigSO : ScriptableObject
{
    public float currentEnergy;
    public float maxEnergy;
    public float rechargeInterval;
    public float rechargeRate;
    public AudioClip fullEnergySFX;
}
