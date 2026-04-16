using UnityEngine;
using UnityEngine.AI;

public class ShotguneerCrackerAttackState : BaseState
{   
    Vector2 _moveOffset;
    readonly Transform _ownerTransform;
    readonly GameObject _owner;
    NavMeshAgent _agent;
    Animator _animator;
    Transform _target;
    readonly EnemyStateData _stateData;
    readonly GameObject _weapon;
    readonly BaseEnemyWeapon _weaponScript;
    readonly WeaponHandsManager _weaponHandsManager;
    readonly AttackPatternSO _attackPattern;
    Coroutine _attackPatternCoroutine;

    // Movement
    float _phase;
    float _speed;
    float _amplitude;

    // Attack Wait Timeout
    float _attackDelayThreshold = 1f;

    public ShotguneerCrackerAttackState(GameObject owner, WeaponHandsManager weaponHandsManager,
    GameObject weapon, GameObject player, NavMeshAgent agent, EnemyStateData ownerStateData, Animator animator, AttackPatternSO attackPattern)
    {   
        _owner = owner;
        _ownerTransform = owner.transform;
        _agent = agent;
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
        
        _amplitude = Random.Range(1f, 3f);
        _speed = Random.Range(1f, 2f);
        _phase = Random.Range(0f, Mathf.PI * 2f);

        //agent.enabled = false;
    }

    public override void StateUpdate()
    {
        DecideMovement();
        //if (_weaponScript != null && _weaponScript.AttackPattern != null && _weaponScript.AttackPattern.IsExecuting != true)
        if (_weaponScript && _attackPattern && _attackPattern.IsOnCooldown == false && _attackPattern.IsExecuting == false && _stateData.HasLineOfSight)
        {
            TryExecuteAttackPattern();
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

    public override void StateFixedUpdate()
    {
        // no op
    }

    public override void OnExit()
    {
        _weaponHandsManager.ResetHands();
        _stateData.IsAttacking = false;
    }

    /*
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
    */



    void DecideMovement()
    {
        if (_stateData.PlayerWithinAttackRange == false)
        {
            //Vector2 target = (Vector2)_target.position + GetBreathingOffset();
            Vector2 toPlayer = (_target.position - _ownerTransform.position).normalized;
            Vector2 perp = new(-toPlayer.y, toPlayer.x);

            Vector2 target = (Vector2)_target.position + perp * Random.Range(-1f, 1f);
            //Debug.Log("Target set to :" + target);
            _agent.SetDestination(target);
            _animator.SetBool("isMoving", true);
        }

        else
        {
            if (_stateData.HasLineOfSight)
            {
                //Debug.Log("HAVE LINE OF SIGHT!");
                _agent.ResetPath();
                _animator.SetBool("isMoving", false);
            }
            else
            {
                // In range but no LoS, reposition
                //Debug.Log("NO LINE OF SIGHT! REPOSITIONING!");
                Vector2 toPlayer = (_target.position - _ownerTransform.position).normalized;
            Vector2 perp = new(-toPlayer.y, toPlayer.x);

            Vector2 target = (Vector2)_target.position + perp * Random.Range(-1f, 1f);
                _agent.SetDestination(target);
                _animator.SetBool("isMoving", true);
            }
        }
    }
}

