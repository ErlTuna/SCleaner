using UnityEngine;
using UnityEngine.AI;

public class RevolverCrackerAttackState : BaseState
{
    GameObject _owner;
    Transform _ownerTransform;
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
        _ownerTransform = owner.transform;
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
        if (_stateData.PlayerWithinAttackRange == false)
        {
            //Vector2 target = (Vector2)_target.position + GetBreathingOffset();
            Vector2 toPlayer = (_target.position - _ownerTransform.position).normalized;
            Vector2 perp = new(-toPlayer.y, toPlayer.x);

            Vector2 target = (Vector2)_target.position + perp * Random.Range(-1f, 1f);
            Debug.Log("Target set to :" + target);
            _agent.SetDestination(target);
            _animator.SetBool("isMoving", true);
        }

        else
        {
            if (_stateData.HasLineOfSight)
            {
                Debug.Log("HAVE LINE OF SIGHT!");
                _agent.ResetPath();
                _animator.SetBool("isMoving", false);
            }
            else
            {
                // In range but no LoS, reposition
                Debug.Log("NO LINE OF SIGHT! REPOSITIONING!");
                Vector2 toPlayer = (_target.position - _ownerTransform.position).normalized;
                Vector2 perp = new(-toPlayer.y, toPlayer.x);

                Vector2 target = (Vector2)_target.position + perp * Random.Range(-1f, 1f);
                _agent.SetDestination(target);
                _animator.SetBool("isMoving", true);
            }
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
