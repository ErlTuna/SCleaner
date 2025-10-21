using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedCrackerAttackState : BaseState
{
    Transform _target;
    EnemyStateData _stateData;
    UnitAttackConfigSO _attackConfig;
    Unit _ownerScript;
    GameObject _weapon;
    BaseEnemyWeapon _weaponScript;
    WeaponHandsManager _weaponHandsManager;
    public RangedCrackerAttackState(GameObject owner, Unit ownerScript, WeaponHandsManager weaponHandsManager, GameObject weapon, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData ownerStateData, UnitAttackConfigSO attackConfigSO) : base(owner, rb2D, agent)
    {
        _ownerScript = ownerScript;
        _target = player.transform;
        _stateData = ownerStateData;
        _attackConfig = attackConfigSO;
        _weapon = weapon;
        _weaponScript = _weapon.GetComponent<BaseEnemyWeapon>();
        _weaponHandsManager = weaponHandsManager;
    }

    public override void OnEnter()
    {
        if (_weaponScript == null)
        {
            Debug.Log("Weapon script is missing?..");
            return;
        }

        if (agent.hasPath) agent.ResetPath();
        if (_weaponHandsManager != null)
            _weaponHandsManager.SetTarget(_target);
     
        //agent.enabled = false;
    }

    public override void StateUpdate()
    {
        DecideMovement();
        if (_weaponScript != null && _weaponScript.AttackPattern != null && _weaponScript.AttackPattern.IsExecuting != true)
        {
            _weaponScript.ExecuteAttackPattern();

            _stateData.IsAttacking = true;
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        _stateData.IsAttacking = false;
    }

    void DecideMovement()
    {
        if (_stateData.PlayerWithinAttackRange != true)
            agent.SetDestination(_target.position);

        else
            agent.ResetPath();
    }

}
