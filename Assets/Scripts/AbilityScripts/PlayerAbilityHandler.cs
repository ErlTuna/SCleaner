using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHandler : MonoBehaviour
{
    public static event Action<AbilityData> OnAbilityUsed;
    public static event Action OnAbilityFinished;
    [SerializeField] List<AbilityData> abilities;
    [SerializeField] AbilityData currentAbility;
    [SerializeField] AbilityContext context;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] PlayerEnergyData _playerEnergyData;
    Coroutine useStateCoroutine;
    public int currentAbilityIndex = 0;
    public UnitStateData _playerStateData;


    void Start()
    {
        UIEvents.RaiseAbilityChanged(currentAbility.UI_Icon);
    }

    public void InitializeManager(PlayerEnergyData playerEnergyData)
    {
        _playerEnergyData = playerEnergyData;
    }

    public void InitializeStateData(UnitStateData playerState)
    {
        _playerStateData = playerState;
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

    }

    Coroutine StartAbilityUse()
    {
        OnAbilityUsed?.Invoke(currentAbility);
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
        
        if (PlayerInputManager.Instance.AbilityUseInput)
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
        UIEvents.RaiseAbilityChanged(currentAbility.UI_Icon);
    }

    void UseAbility()
    {
        List<GameObject> _targets = new();
        _targets.Add(gameObject);

        context = new AbilityContext(gameObject, _targets, PlayerInputManager.Instance.MovementInput, _playerStateData);
        Execute(context);
    }

}


