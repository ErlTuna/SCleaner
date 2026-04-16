using System.Collections.Generic;
using System.Diagnostics;

public class EffectContainer
{
    readonly List<IPersistentEffect> _effects = new();

    public void Add(IPersistentEffect effect, EffectContext ctx)
    {
        _effects.Add(effect);
        effect.Attach(ctx, Remove);
    }

    public void Remove(IPersistentEffect effect)
    {
        if (_effects.Remove(effect))
        {
            effect.Detach();
        }
    }
}
