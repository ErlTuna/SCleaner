using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrackerPreAttack : BaseState
{
    Unit _ownerScript;
    EnemyStateData _stateData;
    UnitMovementData _movementData;
    Transform _target;
    Vector2 _targetDirection;
    float _originalSpeed;
    Coroutine _prepareAttack;

    public CrackerPreAttack(GameObject owner, Unit ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData, UnitMovementData movementData) : base(owner, rb2D, agent)
    {
        _target = player.transform;
        _ownerScript = ownerScript;
        _movementData = movementData;
        _stateData = stateData;
        _originalSpeed = _movementData.CurrentMovementSpeed;
        
    }

    public override void OnEnter()
    {
        agent.speed *= 0.5f;
        _prepareAttack = _ownerScript.StartCoroutine(PrepareAttack());
    }

    public override void OnExit()
    {
        if(_stateData.IsChargingAnAttack){
            CancelCharge();
        }

        agent.speed = _movementData.CurrentMovementSpeed;
    }

    public override void StateUpdate()
    {
        CalculatePlayerDirection();
        RotateTowardsPlayer();
        agent.SetDestination(_target.position);
    }

    public override void StateFixedUpdate()
    {
        //no op
    }

    IEnumerator PrepareAttack()
    {
        _stateData.IsChargingAnAttack = true;
        yield return new WaitForSeconds(.5f);
        Debug.Log("Charged up!");
        _stateData.IsChargingAnAttack = false;
        _stateData.IsAttacking = true;
        _prepareAttack = null;
    }

    void CancelCharge()
    {
        _ownerScript.StopCoroutine(_prepareAttack);
        _prepareAttack = null;
        _stateData.IsChargingAnAttack = false;
        agent.speed = _movementData.CurrentMovementSpeed;
    }

    void CalculatePlayerDirection(){
        _targetDirection = (_target.position - owner.transform.position).normalized;
    }

    void RotateTowardsPlayer(){
        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
        owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
