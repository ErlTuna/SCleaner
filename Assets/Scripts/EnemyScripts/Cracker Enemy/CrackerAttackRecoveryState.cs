using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackRecoveryState : BaseState
{
    private static WaitForSeconds _attackRecoveryDuration = new(.75f);
    GameObject _owner;
    NavMeshAgent _agent;
    Rigidbody2D _rb2D;
    Coroutine _recoveryRoutine;
    MonoBehaviour _ownerScript;
    EnemyStateData _stateData;
    Vector2 targetDirection;
    Transform target;

    public CrackerAttackRecoveryState(GameObject owner, MonoBehaviour ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData)
    {
        _owner = owner;
        _agent = agent;
        _rb2D = rb2D;
        _ownerScript = ownerScript;
        _stateData = stateData;
        target = player.transform;
    }
    public override void OnEnter()
    {
        Debug.Log("entered recovery");
        _stateData.HasAttacked = false;
        _recoveryRoutine = _ownerScript.StartCoroutine(PostAttackRecovery());
    }

    public override void OnExit()
    {
        if (_stateData.CanMove)
        {
            if (_recoveryRoutine != null)
                _ownerScript.StopCoroutine(_recoveryRoutine);
        }
        
        _rb2D.velocity = Vector2.zero;
        _rb2D.angularVelocity = 0f;
        EnableAgent();
    }

    public override void StateFixedUpdate()
    {
        //noop
    }

    public override void StateUpdate()
    {
        //CalculatePlayerDirection();
        //RotateTowardsPlayer();
    }
    IEnumerator PostAttackRecovery()
    {
        Debug.Log("entered attack recovery coroutine");
        _stateData.IsRecoveringPostAttack = true;
        _stateData.CanMove = false;
        yield return _attackRecoveryDuration;
        _stateData.IsRecoveringPostAttack = false;
        _stateData.CanMove = true;
        Debug.Log("exited attack recovery coroutine");
    }
    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - _owner.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        _owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void EnableAgent(){
        _agent.enabled = true;
        _rb2D.isKinematic = true;
        _rb2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
