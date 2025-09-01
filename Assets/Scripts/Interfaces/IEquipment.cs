using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    public static event Action<EquipmentSO> OnEquipmentChanged;
    public static event Action<EquipmentSO> OnEquipmentUsed;
    public static void InvokeEquipmentChangeEvent(EquipmentSO equipmentSO) => OnEquipmentChanged?.Invoke(equipmentSO);
    public static void InvokeEquipmentUseEvent(EquipmentSO equipmentInfo) => OnEquipmentUsed?.Invoke(equipmentInfo);
    public event Action OnAbilityEnd;
    void ActivateScript();
    void DisableScript();
    void SetupEquipmentParameters(Quaternion rotation, Vector2 direction, Transform parentTransform, Transform grenadePosition);
    void UseEquipment();
    void TriggerAbility();
    IEnumerator TriggerAbilityAfterTime();
    EquipmentSO EquipmentInfo { get; set; }
}
