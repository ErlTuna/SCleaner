using UnityEngine;
using UnityEngine.AI;

public class ShotguneerCrackerAttackState : BaseState
{
    readonly GameObject _owner;
    NavMeshAgent _agent;
    Animator _animator;
    Transform _target;
    readonly EnemyStateData _stateData;
    readonly GameObject _weapon;
    readonly BaseEnemyWeapon _weaponScript;
    readonly WeaponHandsManager _weaponHandsManager;
    public ShotguneerCrackerAttackState(GameObject owner, WeaponHandsManager weaponHandsManager,
    GameObject weapon, GameObject player, NavMeshAgent agent, EnemyStateData ownerStateData, Animator animator)
    {
        _owner = owner;
        _agent = agent;
        _target = player.transform;
        _stateData = ownerStateData;
        _weapon = weapon;
        _weaponScript = _weapon.GetComponent<BaseEnemyWeapon>();
        _weaponHandsManager = weaponHandsManager;
        _animator = animator;
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
        if (_weaponScript != null && _weaponScript.AttackPattern != null && _weaponScript.AttackPattern.IsExecuting != true)
        {
            _weaponScript.ExecuteAttackPattern();

            _stateData.IsAttacking = true;
        }
    }

    public override void StateFixedUpdate()
    {
        // no op
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

}

