using System;

public interface IPersistentEffect
{
    event Action<IPersistentEffect> OnConsumed;
    void Attach(EffectContext ctx, Action<IPersistentEffect> onConsumed);
    void Detach();
}
