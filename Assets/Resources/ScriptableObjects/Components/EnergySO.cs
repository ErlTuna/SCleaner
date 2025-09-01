using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy X", menuName = "ScriptableObjects/Component SOs/Energy")]
public class EnergySO : ScriptableObject
{
    public float currentEnergy;
    public float maxEnergy;
    public float rechargeInterval;
    public float rechargeRate;
}
