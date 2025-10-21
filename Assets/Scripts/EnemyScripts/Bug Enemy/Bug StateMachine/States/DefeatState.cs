using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefeatState : BaseState
{
    readonly GameObject visuals;
    DamageContext lastHitContext;
    readonly Sprite _defeatSprite;
    //Vector2 lastHitDirection;
    public DefeatState(GameObject owner, Rigidbody2D rb2D, NavMeshAgent agent, GameObject visuals, Sprite defeatSprite = null) : base(owner, rb2D, agent)
    {
        this.visuals = visuals;
        _defeatSprite = defeatSprite;
    }

    public override void OnEnter()
    {

        // DOESN'T WORK DUE TO SHADER
        //if (visuals.TryGetComponent<SpriteRenderer>(out var renderer))
        //renderer.color = Color.gray;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Rigidbody2D visuals_RB2D = visuals.AddComponent<Rigidbody2D>();
        
        if (visuals.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            if (_defeatSprite != null)
                spriteRenderer.sprite = _defeatSprite; 
        }

        if (visuals_RB2D != null)
        {
            //Vector2 lastHitDirection = ((Vector2)owner.transform.position - lastHitContext.HitPosition).normalized;

            Vector2 lastHitDirection = CalculateHitDirection(lastHitContext.HitPosition);
            visuals_RB2D.isKinematic = false;
            visuals_RB2D.drag = 2.5f;
            visuals_RB2D.velocity = Vector2.zero;
            //Debug.Log("Pushed with " + lastHitContext.PushForce + " in direction : " + lastHitDirection);
            visuals_RB2D.AddForce(lastHitDirection * lastHitContext.PushForce, ForceMode2D.Impulse);
        }


        /*
        if (rb2D != null)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.isKinematic = true;
        }
        */

        if (visuals != null)
        {
            visuals.transform.SetParent(null);
        }

        Object.Destroy(owner);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void SetLastHitContext(DamageContext context)
    {
        //this.lastHitDirection = lastHitDirection;
        lastHitContext = context;
    }



    Vector2 CalculateHitDirection(Vector2 attackPosition)
    {
        Vector2 direction = (Vector2)owner.transform.position - attackPosition;

        if (direction.sqrMagnitude < 0.01f)
        {
            do
            {
                direction = Random.insideUnitCircle;
            } while (direction.sqrMagnitude < 0.01f);

            direction.Normalize();
        }

        else
            direction.Normalize();


        return direction;
    }


}
