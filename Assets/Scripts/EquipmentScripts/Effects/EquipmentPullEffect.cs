using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EquipmentPullEffect : EquipmentEffect
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

    public override void End(EquipmentAbilityContext context)
    {

    }
}
