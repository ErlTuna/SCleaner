using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CrackerAttackRecoveryState : BaseState
{
    Coroutine _recoveryRoutine;
    Unit _ownerScript;
    EnemyStateData _stateData;
    Vector2 targetDirection;
    Transform target;

    public CrackerAttackRecoveryState(GameObject owner, Unit ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData) : base(owner, rb2D, agent){
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
        if(_stateData.CanMove){
            if(_recoveryRoutine != null)
                _ownerScript.StopCoroutine(_recoveryRoutine);
        }
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f;
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
        yield return new WaitForSeconds(1.5f);
        _stateData.IsRecoveringPostAttack = false;
        _stateData.CanMove = true;
        Debug.Log("exited attack recovery coroutine");
    }
    void CalculatePlayerDirection(){
        //calculate direction
        targetDirection = (target.position - owner.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void EnableAgent(){
        agent.enabled = true;
        rb2D.isKinematic = true;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
