using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VacuumNade : BaseEquipment
{
    [SerializeField] float _throwStrength;
    [SerializeField] EnemyDetectionHandler _enemyDetectionHandler;
    [SerializeField] Rigidbody2D _rb2D;
    Vector2 _trajectory;
    Transform _parent;

    void Update()
    {
        if (EquipmentData.State == EquipmentState.DEPLOYING && _rb2D.velocity.sqrMagnitude < 0.01f)
        {
            OnActivation();
        }

        else if (EquipmentData.State == EquipmentState.ACTIVE)
        {
            EquipmentData.Lifetime -= Time.deltaTime;
            ApplyOvertimeEffects();

            if (EquipmentData.Lifetime <= 0.01f)
            {
                Debug.Log("Lifetime over");
                OnDeactivation();
            }
            
        }
    }

    public override void OnUse()
    {
        if (_enemyDetectionHandler == null) return;

        gameObject.SetActive(true);
        EquipmentData.State = EquipmentState.DEPLOYING;


        transform.parent = null;
        Throw();
        
    }
    public override void OnActivation()
    {
        Debug.Log("Activating");
        _rb2D.velocity = Vector2.zero;
        _enemyDetectionHandler.EnableCollider(true);
        EquipmentData.State = EquipmentState.ACTIVE;
        StartCoroutine(ApplyActivationEffects());
    }

    IEnumerator ApplyActivationEffects()
    {
        // waiting until fixed update for physics simulation to catch up
        yield return new WaitForFixedUpdate();

        List<GameObject> caughtEnemies = _enemyDetectionHandler.EnemyGOs.ToList();
        EquipmentAbilityContext context = new(gameObject, EquipmentData, caughtEnemies);

        foreach (EquipmentEffect effect in EquipmentConfig.Ability.ActivationEffects)
        {
            Debug.Log("Attempting activation effect execution");
            effect.Execute(context, EquipmentData);
        }
    }

    void ApplyOvertimeEffects()
    {
        List<GameObject> caughtEnemies = _enemyDetectionHandler.EnemyGOs.ToList();
        EquipmentAbilityContext context = new(gameObject, EquipmentData, caughtEnemies);

        foreach (EquipmentEffect effect in EquipmentConfig.Ability.OverTimeEffects)
            effect.Tick(context, EquipmentData);
        
    }

    void ApplyExpirationEffects()
    {
        List<GameObject> caughtEnemies = _enemyDetectionHandler.EnemyGOs.ToList();
        EquipmentAbilityContext context = new(gameObject, EquipmentData, caughtEnemies);

        foreach (EquipmentEffect effect in EquipmentConfig.Ability.ExpirationEffects)
            effect.Execute(context, EquipmentData);
        
    }


    public override void OnDeactivation()
    {
        ApplyExpirationEffects();
        EquipmentData.State = EquipmentState.INACTIVE;
        EquipmentData.Lifetime = EquipmentConfig.MaxLifetime;
        gameObject.transform.position = _parent.transform.position;
        gameObject.transform.parent = _parent;
        _enemyDetectionHandler.EnableCollider(false);
        gameObject.SetActive(false);
    }

    public override void OnReturn()
    {
        // no op
    }

    public override void InitializeDeployment(Quaternion rotation, Vector2 direction, Transform parentTransform, Vector2 grenadePosition)
    {
        transform.rotation = rotation;
        _trajectory = direction.normalized;
        _parent = parentTransform;
        transform.position = grenadePosition;
    }

    public override bool CanUseEquipment()
    {
        return EquipmentData.CurrentCharge > 0 && EquipmentData.State == EquipmentState.INACTIVE;
    }

    public void Throw()
    {
        _rb2D.AddForce(_trajectory * _throwStrength, ForceMode2D.Impulse);
    }
}

