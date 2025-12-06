using System;
using System.Numerics;

public interface IDamageable
{
    void TakeDamage(int amount);
    void TakeDamage(DamageContext context);
}

public interface IDefeatable
{
    event Action OnDefeat;
    event Action<DamageContext> OnDefeatContext;
}

public interface IDetachable
{
    void Detach();
    void Detach(DamageContext damageContext);
}

public interface IFactionMember
{
    public Faction Faction { get; }
}
