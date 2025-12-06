using System;

[Serializable]
public abstract class EquipmentEffect
{
    public abstract void Execute(EquipmentAbilityContext context, EquipmentData equipmentData);
    public abstract void Tick(EquipmentAbilityContext context, EquipmentData equipmentData);
    public abstract void End(EquipmentAbilityContext context);
}