using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHandler : MonoBehaviour
{
    public static event Action<AbilityData> OnAbilityUsed;
    public static event Action OnAbilityFinished;
    [SerializeField] List<AbilityData> abilities;
    [SerializeField] AbilityData currentAbility;
    [SerializeField] AbilityContext context;
    [SerializeField] PlayerCoreSO userInfo;
    Coroutine useStateCoroutine;
    public int currentAbilityIndex = 0;


    public void Execute(AbilityContext context)
    {
        if (currentAbility.CanBeUsed() && userInfo.energyData.currentEnergy - currentAbility.energyCost >= 0)
        {
            if (useStateCoroutine == null) useStateCoroutine = StartAbilityUse();

            foreach (AbilityEffect effect in currentAbility.effects)
                effect.Execute(context, currentAbility);
        }
    }

    Coroutine StartAbilityUse()
    {
        OnAbilityUsed?.Invoke(currentAbility);
        //UIEvents.RaiseEnergyUsed();
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
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            context = new AbilityContext { user = gameObject, target = gameObject, direction = PlayerInputManager.instance.MovementInput, customData = { ["StateData"] = userInfo.stateData } };
            Execute(context);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            ChangeAbility();
        }
    }

    void ChangeAbility()
    {
         Debug.Log("Changing abilities");
        ++currentAbilityIndex;
        currentAbility = abilities[currentAbilityIndex];
        UIEvents.RaiseAbilityChanged(currentAbility);
    }
}
