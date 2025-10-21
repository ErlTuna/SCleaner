using UnityEngine;
using UnityEngine.AI;

public class RangedCrackerPreAttackState : BaseState
{
    Transform _target;
    EnemyStateData _stateData;
    Unit _ownerScript;
    WeaponHandsManager _weaponHandsManager;


    public RangedCrackerPreAttackState(GameObject owner, Unit ownerScript, WeaponHandsManager weaponHandsManager, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData ownerStateData) : base(owner, rb2D, agent)
    {
        _ownerScript = ownerScript;
        _target = player.transform;
        _stateData = ownerStateData;
        _weaponHandsManager = weaponHandsManager;
    }

    public override void OnEnter()
    {
        if (agent.hasPath) agent.ResetPath();
        if (agent.enabled == false) agent.enabled = true;
        agent.SetDestination(_target.position);
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
        agent.enabled = true;
        Debug.Log("ranged cracker exited [PRE-ATTACK] state");

        _stateData.IsChargingAnAttack = false;
        //_weaponHandsManager.SetTarget(null);
    }
}
