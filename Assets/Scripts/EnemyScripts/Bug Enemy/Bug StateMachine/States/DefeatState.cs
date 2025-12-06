using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.AI;

public class DefeatState : BaseState
{
    readonly GameObject _owner;
    readonly NavMeshAgent _agent;
    readonly GameObject _visualsGO;
    DamageContext _lastHitContext;
    readonly EnemyVisualConfigSO _visualConfig;
    readonly Rigidbody2D _visualsRB;
    public DefeatState(GameObject owner, Rigidbody2D visualsRB, NavMeshAgent agent, GameObject visuals, EnemyVisualConfigSO visualConfig = null)
    {
        _owner = owner;
        _agent = agent;
        _visualsRB = visualsRB;
        _visualsGO = visuals;
        _visualConfig = visualConfig;
    }

    public override void OnEnter()
    {

        if (_agent != null && _agent.enabled)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        if (_visualsGO.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            if (_visualConfig != null)
                spriteRenderer.sprite = _visualConfig.DefeatedBodyAppearance;
            else
                Debug.Log("defeat sprite null?");
        }
        
        if (_visualsGO != null)
        {
            _visualsGO.transform.SetParent(null);
        }

        if (_visualsRB != null)
        {
            _visualsRB.simulated = true;
            _visualsRB.isKinematic = false;
            _visualsRB.drag = 2.5f;
            _visualsRB.velocity = Vector2.zero;
            Debug.Log("The visuals rb " + _visualsRB);
            Debug.Log("Adding force : " + _lastHitContext.HitterMovementVector * _lastHitContext.PushForce);
            _visualsRB.AddForce(_lastHitContext.HitterMovementVector * _lastHitContext.PushForce, ForceMode2D.Impulse);
        }

        Object.Destroy(_owner);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void SetLastHitContext(DamageContext context)
    {
        _lastHitContext = context;
    }

}
