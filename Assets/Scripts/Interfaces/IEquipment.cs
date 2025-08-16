using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    public event Action OnAbilityEnd;
    void ActivateScript();
    void DisableScript();
    void SetupEquipmentParameters(Quaternion rotation, Vector2 direction, Transform parentTransform, Transform grenadePosition);
    void UseEquipment();
    void TriggerAbility();
    IEnumerator TriggerAbilityAfterTime();
}
