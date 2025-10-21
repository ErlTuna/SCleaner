using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Configs Wrapper", menuName = "ScriptableObjects/Unit Config/Configs Wrapper")]
public class UnitConfigsWrapperSO : ScriptableObject
{
    public string UnitName;
    public Sprite defeatSprite;
    public UnitHealthConfigSO HealthConfig;
    public UnitMovementConfigSO MovementConfig;
    public UnitEnergyConfigSO EnergyConfig;
    public UnitStateConfigSO StateConfig;
    public UnitInventoryConfigSO InventoryConfig;
    public UnitAttackConfigSO attackConfigSO;
}
