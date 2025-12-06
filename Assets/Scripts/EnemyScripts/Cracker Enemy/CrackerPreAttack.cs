using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrackerPreAttack : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Rigidbody2D _rb2D;
    MonoBehaviour _ownerScript;
    EnemyStateData _stateData;
    UnitMovementData _movementData;
    Transform _target;
    float _originalSpeed;
    Coroutine _prepareAttack;

    public CrackerPreAttack(GameObject owner, Unit ownerScript, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData stateData, UnitMovementData movementData)
    {
        _owner = owner;
        _agent = agent;
        _rb2D = rb2D;
        _target = player.transform;
        _ownerScript = ownerScript;
        _movementData = movementData;
        _stateData = stateData;
        _originalSpeed = _movementData.CurrentMovementSpeed;
        
    }

    public override void OnEnter()
    {
        _agent.speed *= 0.5f;
        _prepareAttack = _ownerScript.StartCoroutine(PrepareAttack());
    }

    public override void OnExit()
    {
        if(_stateData.IsChargingAnAttack){
            CancelCharge();
        }

        _agent.speed = _movementData.CurrentMovementSpeed;
    }

    public override void StateUpdate()
    {
        RotateTowardsPlayer();
        _agent.SetDestination(_target.position);
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
        _agent.speed = _movementData.CurrentMovementSpeed;
    }
    void RotateTowardsPlayer(){

        // subtracting 90f to fix rotation issue as the sprite faces up
        // without this, the enemy rotates 90 degrees more than it should
        Vector2 _targetDirection = (_target.position - _owner.transform.position).normalized;
        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg -90f;
        _owner.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
