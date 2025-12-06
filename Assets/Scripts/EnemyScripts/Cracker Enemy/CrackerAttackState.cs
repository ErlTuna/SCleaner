using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Rigidbody2D _rb2D;
    Coroutine attackCoroutine;
    MonoBehaviour _ownerScript;
    Transform _target;
    Vector2 _targetDirection;
    EnemyStateData _stateData;
    AfterImageEmitter _afterImageEmitter;
    public CrackerAttackState(GameObject owner, MonoBehaviour ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData, AfterImageEmitter emitter)
    {
        _owner = owner;
        _agent = agent;
        _rb2D = rb2D;
        _afterImageEmitter = emitter;
        _target = player.transform;
        _ownerScript = ownerScript;
        _stateData = stateData;
    }
    public override void OnEnter()
    {
        Debug.Log("Entered Cracker Attack");
        //agent.ResetPath();
        //rb2D.isKinematic = false;
        //rb2D.bodyType = RigidbodyType2D.Dynamic;
        //agent.enabled = false;
        DisableAgent();
        
    }

    public override void OnExit()
    {
        Debug.Log("Exited Cracker Attack");
        //rb2D.velocity = Vector2.zero;
        //rb2D.angularVelocity = 0f;
    }

    public override void StateUpdate()
    {
        //no op
    }

    public override void StateFixedUpdate()
    {
        if (_stateData.IsAttacking)
        {
            CalculatePlayerDirection();
            Debug.Log("Launching");
            _rb2D.AddForce(_targetDirection * 7.5f, ForceMode2D.Impulse);
            _ownerScript.StartCoroutine(AttackEnd());
        }
        _afterImageEmitter.TryEmit();
    }

    IEnumerator AttackEnd()
    {
        _stateData.IsAttacking = false;
        Debug.Log("Attack end!!");
        yield return new WaitForSeconds(1.5f);
        _stateData.HasAttacked = true;
    }

    void CalculatePlayerDirection(){
        //calculate direction
        _targetDirection = (_target.position - _owner.transform.position).normalized;
    }
    void DisableAgent(){
        _agent.ResetPath();
        _agent.enabled = false;
        _rb2D.isKinematic = false;
        _rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

}
