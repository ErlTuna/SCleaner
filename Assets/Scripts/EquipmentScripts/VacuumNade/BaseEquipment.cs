using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseEquipment : MonoBehaviour
{
    public GameObject User;
    public EquipmentConfigSO EquipmentConfig;
    public EquipmentData EquipmentData;
    public abstract void InitializeDeployment(Quaternion rotation, Vector2 direction, Transform parentTransform, Vector2 spawnPosition);
    public abstract void OnDeployment();
    public abstract void OnActivation();
    public abstract void OnDeactivation();
    public virtual bool CanUseEquipment()
    {
        return true;
    }
    public virtual void InitializeWithData()
    {
        EquipmentData = new EquipmentData(EquipmentConfig);
    }

}


[Serializable]
public class EquipmentData
{
    public string EquipmentName;
    public EquipmentConfigSO EquipmentConfig;
    public EquipmentState State;
    public EquipmentAbilityData Ability;
    public int CurrentCount;
    public int MaxCount;
    public float Lifetime;


    public EquipmentData(EquipmentConfigSO equipmentConfigSO)
    {
        State = EquipmentState.INACTIVE;
        EquipmentConfig = equipmentConfigSO;
        MaxCount = EquipmentConfig.MaxCount;
        CurrentCount = EquipmentConfig.StartingCount;
        Lifetime = EquipmentConfig.MaxLifetime;
        Ability = equipmentConfigSO.Ability;
    }

}

[Serializable]
public enum EquipmentState
{
    INACTIVE,
    DEPLOYING,
    ACTIVE
}

[Serializable]
public enum EquipmentType
{
    THROWABLE
}