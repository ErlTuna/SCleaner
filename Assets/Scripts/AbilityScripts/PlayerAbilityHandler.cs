using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityHandler : MonoBehaviour
{
    Unit _owner;
    public static event Action<AbilityData> OnAbilityUsed;
    public static event Action OnAbilityFinished;
    [SerializeField] List<AbilityData> abilities;
    [SerializeField] AbilityData currentAbility;
    [SerializeField] AbilityContext context;
    [SerializeField] SpriteRenderer _spriteRenderer;
    UnitEnergyData _playerEnergyData;
    Coroutine useStateCoroutine;
    public int currentAbilityIndex = 0;


    void Start()
    {
        UIEvents.RaiseAbilityChanged(currentAbility);
    }


    public void Execute(AbilityContext context)
    {

        if (currentAbility.CanBeUsed(context) != true) return;
        if (_playerEnergyData.CurrentEnergy - currentAbility.EnergyCost < 0)
        {
            Debug.Log("Not enough energy");
            return;
        }

        if (useStateCoroutine == null) useStateCoroutine = StartAbilityUse();

        foreach (AbilityEffect effect in currentAbility.Effects)
            effect.Execute(context, currentAbility);

        /*
        if (currentAbility.AbilityVisuals != null)
        {
            currentAbility.AbilityVisuals.TriggerVisual(this, context.user.transform.position, Quaternion.identity, _spriteRenderer);
        }
        */

    }

    Coroutine StartAbilityUse()
    {
        OnAbilityUsed?.Invoke(currentAbility);
        UIEvents.RaiseEnergyUsed();
        return StartCoroutine(currentAbility.AbilityTriggered(AbilityFinished));
    }

    void AbilityFinished()
    {
        if (useStateCoroutine != null)
            useStateCoroutine = null;

        OnAbilityFinished?.Invoke();
    }

    void Update()
    {
        /*
        if (PlayerInputManager.instance.AbilitySwitchInput)
        {
            ChangeAbility();
        }
        */
        
        if (PlayerInputManager.instance.AbilityUseInput)
        {
            UseAbility();
        }


    }

    void ChangeAbility()
    {
        if (useStateCoroutine != null)
        {
            Debug.Log("Can't change abilities while an ability is active!");
            return;
        }

        Debug.Log("Changing abilities");
        currentAbilityIndex = (currentAbilityIndex + 1) % abilities.Count;
        currentAbility = abilities[currentAbilityIndex];
        UIEvents.RaiseAbilityChanged(currentAbility);
    }

    void UseAbility()
    {
        List<GameObject> _targets = new();
        _targets.Add(gameObject);

        context = new AbilityContext
            {
                user = gameObject,
                targets = _targets,
                direction = PlayerInputManager.instance.MovementInput,
                userRuntimeData = _owner.RuntimeDataHolder
            };
        Execute(context);
    }

    public void InitializeWithData(Unit owner, UnitEnergyData energyData)
    {
        _owner = owner;
        _playerEnergyData = energyData;
    }
}


