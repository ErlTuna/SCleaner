using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class EquipmentEffect
{
    public EquipmentEffectApplicationType ApplicationType;
    public abstract void Execute(EquipmentAbilityContext context, EquipmentData equipmentData);
    public abstract void Tick(EquipmentAbilityContext context, EquipmentData equipmentData);
    public abstract void End(EquipmentAbilityContext context);
}

[Serializable]
class DamageEffect : EquipmentEffect
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
            DamageContext hitContext = new(useContext.User, useContext.User.transform.position, Damage, PushForce);
            //Debug.Log("Damaged enemies for " + Damage);
            damageable.TakeDamage(hitContext);
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

[Serializable]
class PullEffect : EquipmentEffect
{
    public float Duration = 1f;
    public float PullForce = 1f;
    public float PullDuration = 1f;
    public float PullRadius = 5f;

    HashSet<Rigidbody2D> _rigidbodies = new();
    HashSet<Unit> _enemyScripts = new();
    

    public override void Execute(EquipmentAbilityContext context, EquipmentData equipmentData)
    {


    }

    public override void End(EquipmentAbilityContext context)
    {

    }



    void PullTowardsCenter(EquipmentAbilityContext context, HashSet<Rigidbody2D> rigidbodies, HashSet<Unit> enemyScripts)
    {

        if (rigidbodies.Count == 0) return;
        
        //Vector2 individualPullForce;
        Vector2 direction;
        Vector2 suctionCenter = context.User.transform.position;


        for (int i = 0; i < rigidbodies.Count(); ++i)
        {
            Rigidbody2D rb2d = rigidbodies.ElementAt(i);

            if (rb2d != null)
            {
                direction = suctionCenter - rb2d.position;
                float distance = direction.magnitude;
                direction.Normalize();


                if (distance < 0.5f)
                {
                    rb2d.velocity = Vector2.zero;
                }

                else
                {
                    float forceMultiplier = Mathf.Clamp01(distance / PullRadius);
                    Vector2 individualPullForce = direction * (PullForce * forceMultiplier);

                    rb2d.AddForce(individualPullForce, ForceMode2D.Force);
                }

            }

        }
        
    }

    public override void Tick(EquipmentAbilityContext context, EquipmentData equipmentData)
    {
        Duration = equipmentData.Lifetime;

        if (Duration < 0.01f)
        {
            Debug.Log("Equipment life time ended");
            End(context);
            return;
        }

        if (context.AffectedTargets.Count == 0)
        {
            Debug.Log("No targets to pull.");
            return;
        }

        foreach (GameObject target in context.AffectedTargets)
        {
            if (target.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
            {
                if (_rigidbodies.Add(rigidbody2D))
                {
                    rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                if (target.TryGetComponent<Unit>(out var enemyScript))
                {
                    _enemyScripts.Add(enemyScript);
                }

            }
        }

        PullTowardsCenter(context, _rigidbodies, _enemyScripts);
    }
}

[Serializable]
class ImmobilizeEffect : EquipmentEffect
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



        foreach (GameObject target in context.AffectedTargets)
        {
            if (target.TryGetComponent<Unit>(out var enemyScript))
            {
                if (enemyScript.TryGetRuntimeData(out EnemyStateData enemyStateData))
                {
                    _enemyStateDatas.Add(enemyStateData);
                    if (enemyStateData.CanMove == true)
                    {
                        enemyStateData.CanMove = false;
                        Debug.Log("Immobilized an enemy");
                    }
                }
            }
        }
    }

}

class HealEffect : EquipmentEffect
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


public enum EquipmentEffectApplicationType
{
    INSTANTENOUS,
    OVERTIME
}
