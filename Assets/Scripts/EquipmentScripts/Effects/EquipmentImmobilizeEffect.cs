using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentImmobilizeEffect : EquipmentEffect
{
    public float Duration = 1f;
    HashSet<EnemyStateData> _enemyStateDatas = new();
    public override void End(EquipmentAbilityContext context)
    {
        foreach (EnemyStateData enemyState in _enemyStateDatas)
        {
            Debug.Log("freed target");
            enemyState.CanMove = true;

        }
    }

    public override void Execute(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        foreach (GameObject target in context.AffectedTargets)
        {
            if (target.TryGetComponent<Unit>(out var enemyScript))
            {
                EnemyStateData enemyStateData = enemyScript.GetStateData() as EnemyStateData;
                if (enemyStateData != null )
                {
                    _enemyStateDatas.Add(enemyStateData);
                    enemyStateData.CanMove = false;
                    Debug.Log("Immobilized an enemy");
                }
            }
        }
    }

    public override void Tick(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        Duration = equipmentData.Lifetime;

        if (Duration < 0.01f)
        {
            Debug.Log("Equipment life time ended. Ending immobilize effect.");
            End(context);
            return;
        }


        
    }
}
