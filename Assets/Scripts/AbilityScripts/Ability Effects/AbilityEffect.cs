using System;

[Serializable]
public abstract class AbilityEffect
{
    public abstract void Execute(AbilityContext context, AbilityData abilityData);
    public abstract void End(AbilityContext context);
}
