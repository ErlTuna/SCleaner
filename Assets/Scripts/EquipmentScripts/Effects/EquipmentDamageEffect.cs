using UnityEngine;

public class EquipmentDamageEffect : EquipmentEffect
{
    public int Damage;
    public float PushForce = 10f;
    public override void Execute(EquipmentAbilityContext useContext, EquipmentData equipmentData)
    {

        if (useContext.AffectedTargets.Count == 0)
        {
            //Debug.Log("No targets to apply damage to");
            return;
        }

        foreach (GameObject target in useContext.AffectedTargets)
        {
            IDamageable damageable = target.GetComponentInChildren<IDamageable>();
            DamageContext damageContext = new(useContext.User, useContext.User.transform.position.normalized, useContext.User.transform.position, Damage, PushForce);
            damageable.TakeDamage(damageContext);
        }
    }

    public override void End(EquipmentAbilityContext context)
    {
        //no op
    }

    public override void Tick(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        //no op
    }
}
