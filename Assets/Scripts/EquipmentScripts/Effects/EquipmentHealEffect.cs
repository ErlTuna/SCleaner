using System;
using UnityEngine;

[Serializable]
public class EquipmentHealEffect : EquipmentEffect
{
    public int Amount = 0;

    public override void End(EquipmentAbilityContext context)
    {
        //no op
    }

    public override void Execute(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        foreach (GameObject target in context.AffectedTargets)
        {
            IHealable healable = target.GetComponentInChildren<IHealable>();
            if (healable != null)
                healable.Heal(Amount);

        }
    }

    public override void Tick(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        //no op
    }
}
