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
    PlayerEnergyData _playerEnergyData;
    Coroutine useStateCoroutine;
    public int currentAbilityIndex = 0;


    void Start()
    {
        UIEvents.RaiseAbilityChanged(currentAbility);
    }


    public void Execute(AbilityContext context)
    {

        if (currentAbility.CanBeUsed(context) != true) return;
        if (_playerEnergyData.CurrentEnergy - currentAbility.energyCost < 0)
        {
            Debug.Log("Not enough energy");
            return;
        }

        if (useStateCoroutine == null) useStateCoroutine = StartAbilityUse();

        foreach (AbilityEffect effect in currentAbility.effects)
            effect.Execute(context, currentAbility);

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
        if (PlayerInputManager.instance.AbilitySwitchInput)
        {
            ChangeAbility();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            context = new AbilityContext
            { user = _owner, target = _owner, direction = PlayerInputManager.instance.MovementInput, userRuntimeData = _owner.RuntimeDataHolder};
            Execute(context);
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

    public void InitializeWithData(Unit owner, PlayerEnergyData energyData)
    {
        _owner = owner;
        _playerEnergyData = energyData;
    }
}

public enum AbilityType
{
    INSTANTENOUS,
    OVERTIME
}
