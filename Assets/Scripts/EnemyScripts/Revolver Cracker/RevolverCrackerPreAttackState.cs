using UnityEngine;
using UnityEngine.AI;

public class RangedCrackerPreAttackState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Transform _target;
    EnemyStateData _stateData;
    MonoBehaviour _ownerScript;
    WeaponHandsManager _weaponHandsManager;


    public RangedCrackerPreAttackState(GameObject owner, MonoBehaviour ownerScript, WeaponHandsManager weaponHandsManager, GameObject player, NavMeshAgent agent, EnemyStateData ownerStateData)
    {
        _owner = owner;
        _agent = agent;
        _ownerScript = ownerScript;
        _target = player.transform;
        _stateData = ownerStateData;
        _weaponHandsManager = weaponHandsManager;
    }

    public override void OnEnter()
    {
        if (_agent.hasPath) _agent.ResetPath();
        if (_agent.enabled == false) _agent.enabled = true;

        _agent.SetDestination(_target.position);
        Debug.Log("ranged cracker entered pre-attack state, moving towards : " + _target.name);
        _stateData.IsChargingAnAttack = true;
        _weaponHandsManager.SetTarget(_target);
    }

    public override void StateUpdate()
    {
        //base.StateUpdate();
    }

    public override void StateFixedUpdate()
    {
        //base.StateFixedUpdate();
    }

    public override void OnExit()
    {
        _agent.enabled = true;
        Debug.Log("ranged cracker exited [PRE-ATTACK] state");

        _stateData.IsChargingAnAttack = false;
        //_weaponHandsManager.SetTarget(null);
    }
}
