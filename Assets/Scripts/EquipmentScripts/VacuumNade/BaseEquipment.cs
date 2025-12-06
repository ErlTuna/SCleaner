using System;
using UnityEngine;

[Serializable]
public abstract class BaseEquipment : MonoBehaviour
{
    public GameObject Owner;
    public EquipmentConfigSO EquipmentConfig;
    public EquipmentData EquipmentData;
    public abstract void InitializeDeployment(Quaternion rotation, Vector2 direction, Transform parentTransform, Vector2 spawnPosition);
    public abstract void OnUse();
    public abstract void OnActivation();
    public abstract void OnDeactivation();
    public abstract void OnReturn();
    public virtual bool CanUseEquipment()
    {
        return true;
    }
    public virtual void InitializeEquipment()
    {
        EquipmentData = new EquipmentData(EquipmentConfig);
    }

    public virtual void Recharge()
    {
        EquipmentData.CurrentCharge = Mathf.Min(EquipmentData.CurrentCharge + 0.1f, EquipmentData.MaxCharge);
    }
    

}