using System;
// For subtraction, add a negative value. For division, multiply with a fraction.
public enum ModifierOperation { Add, Multiply } 

[Serializable]
public class IntModifier
{
    public int Amount;
    public ModifierOperation Operation;

    public IntModifier(int amount, ModifierOperation operation)
    {
        Amount = amount;
        Operation = operation;
    }

    public int Apply(int current)
    {
        return Operation switch
        {
            ModifierOperation.Add => current + Amount,
            ModifierOperation.Multiply => current * Amount,
            _ => current
        };
    }
}

