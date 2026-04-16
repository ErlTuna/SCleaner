using UnityEngine;
using UnityEngine.AI;

public class DefeatState : BaseState
{
    readonly GameObject _owner;
    readonly NavMeshAgent _agent;
    readonly GameObject _visualsGO;
    DamageContext _lastHitContext;
    readonly EnemyVisualConfigSO _visualConfig;
    readonly Rigidbody2D _bodyRB;
    public DefeatState(GameObject owner, Rigidbody2D bodyRB, NavMeshAgent agent, GameObject visuals, EnemyVisualConfigSO visualConfig = null)
    {
        _owner = owner;
        _agent = agent;
        _bodyRB = bodyRB;
        _visualsGO = visuals;
        _visualConfig = visualConfig;
    }

    public override void OnEnter()
    {

        if (_agent && _agent.enabled)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        if (_visualsGO.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            if (_visualConfig != null)
                spriteRenderer.sprite = _visualConfig.DefeatedBodyAppearance;
            else
                Debug.Log("defeat sprite null?");
        }
        
        if (_visualsGO)
        {
            _visualsGO.transform.SetParent(_owner.transform.parent);
        }

        if (_bodyRB)
        {
            Debug.Log("Adding the force...");
            _bodyRB.simulated = true;
            _bodyRB.isKinematic = false;
            _bodyRB.drag = 2.5f;
            _bodyRB.angularDrag = .5f;
            _bodyRB.velocity = Vector2.zero;
            _bodyRB.AddForce(_lastHitContext.HitterMovementVector * _lastHitContext.PushForce, ForceMode2D.Impulse);
        }

        else
            Debug.Log("Null doe?");

        Object.Destroy(_owner);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void SetLastHitContext(DamageContext context)
    {
        Debug.Log("Last hit context is set.");
        _lastHitContext = context;
    }

}
