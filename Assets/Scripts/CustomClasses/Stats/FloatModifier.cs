public class FloatModifier
{
    public float Amount;
    public ModifierOperation Operation;

    public FloatModifier(float amount, ModifierOperation operation)
    {
        Amount = amount;
        Operation = operation;
    }

    public float Apply(float current)
    {
        return Operation switch
        {
            ModifierOperation.Add => current + Amount,
            ModifierOperation.Multiply => (int)(current * Amount),
            _ => current
        };
    }

}
