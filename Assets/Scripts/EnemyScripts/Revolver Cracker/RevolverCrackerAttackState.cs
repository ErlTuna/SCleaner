using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RevolverCrackerAttackState : BaseState
{
    GameObject _owner;
    NavMeshAgent _agent;
    Rigidbody2D _rb2D;
    Animator _animator;
    Transform _target;
    EnemyStateData _stateData;
    MonoBehaviour _ownerScript;
    GameObject _weapon;
    BaseEnemyWeapon _weaponScript;
    readonly AttackPatternSO _attackPattern;
    WeaponHandsManager _weaponHandsManager;
    Coroutine _attackPatternCoroutine;
    public RevolverCrackerAttackState(GameObject owner, MonoBehaviour ownerScript, WeaponHandsManager weaponHandsManager,
    GameObject weapon, GameObject player, Rigidbody2D rb2D, NavMeshAgent agent, EnemyStateData ownerStateData, Animator animator, AttackPatternSO attackPattern)
    {
        _owner = owner;
        _agent = agent;
        _rb2D = rb2D;
        _ownerScript = ownerScript;
        _target = player.transform;
        _stateData = ownerStateData;
        _weapon = weapon;
        _weaponScript = _weapon.GetComponent<BaseEnemyWeapon>();
        _weaponHandsManager = weaponHandsManager;
        _animator = animator;
        _attackPattern = attackPattern;
    }

    public override void OnEnter()
    {
        if (_weaponScript == null)
        {
            Debug.Log("Weapon script is missing?..");
            return;
        }

        if (_agent.hasPath) _agent.ResetPath();
        if (_weaponHandsManager != null)
            _weaponHandsManager.SetTarget(_target);
     
        //agent.enabled = false;
    }

    public override void StateUpdate()
    {
        DecideMovement();
        //if (_weaponScript != null && _weaponScript.AttackPattern != null && _weaponScript.AttackPattern.IsExecuting != true)
        if (_weaponScript && _attackPattern && _attackPattern.IsOnCooldown == false && _attackPattern.IsExecuting == false)
        {
            TryExecuteAttackPattern();
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
        {
            _agent.SetDestination(_target.position);
            _animator.SetBool("isMoving", true);
        }


        else
        {
            _agent.ResetPath();
            _animator.SetBool("isMoving", false);
        }
            
    }

    void TryExecuteAttackPattern()
    {
        if (_weaponScript.CanFire())
        {
            _stateData.IsAttacking = true;
            _attackPatternCoroutine = _weaponScript.StartCoroutine(_attackPattern.Execute(_weaponScript));
        }
    }

}
