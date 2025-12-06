using System.Collections;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    Unit _owner;
    [SerializeField] UnitEnergyConfigSO _playerEnergyConfig;
    [SerializeField] UnitEnergyData _playerEnergyData;
    Coroutine rechargeCoroutine;
    bool isRecharging = false;

    public void InitializeManager(UnitEnergyData playerEnergyData, UnitEnergyConfigSO playerEnergyConfig)
    {
        _playerEnergyData = playerEnergyData;
        _playerEnergyConfig = playerEnergyConfig;
    }

    void OnEnable()
    {
        PlayerAbilityHandler.OnAbilityFinished += TriggerRecharge;
        PlayerAbilityHandler.OnAbilityUsed += OnEnergyUse;
    }

    void OnDisable()
    {
        PlayerAbilityHandler.OnAbilityFinished -= TriggerRecharge;
        PlayerAbilityHandler.OnAbilityUsed -= OnEnergyUse;
    }

    public void TriggerRecharge()
    {
        if (CanRecharge() == false) return;
        rechargeCoroutine = StartCoroutine(RechargeOvertime());

    }

    public IEnumerator RechargeOvertime()
    {
        while (_playerEnergyData.CurrentEnergy < _playerEnergyData.MaxEnergy)
        {
            isRecharging = true;
            _playerEnergyData.CurrentEnergy += _playerEnergyData.RechargeRate;

            if (_playerEnergyData.CurrentEnergy >= _playerEnergyData.MaxEnergy)
            {
                _playerEnergyData.CurrentEnergy = _playerEnergyData.MaxEnergy;
                UIEvents.RaiseEnergyChanged(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
                AudioSource.PlayClipAtPoint(_playerEnergyConfig.fullEnergySFX, gameObject.transform.position);
                isRecharging = false;
                yield break;
            }

            UIEvents.RaiseEnergyChanged(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
            yield return new WaitForSeconds(.1f);
        }

        isRecharging = false;
        rechargeCoroutine = null;
    }

    public bool CanRecharge()
    {
        return _playerEnergyData.CurrentEnergy != _playerEnergyData.MaxEnergy && !isRecharging;
    }

    public void OnEnergyUse(AbilityData abilityData)
    {
        float energyAfterUse = _playerEnergyData.CurrentEnergy - abilityData.EnergyCost;
        if (Mathf.Approximately(energyAfterUse, 0))
        {
            _playerEnergyData.CurrentEnergy = 0;
            UIEvents.RaiseEnergyChanged(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
            return;
        }

        _playerEnergyData.CurrentEnergy -= abilityData.EnergyCost;
        UIEvents.RaiseEnergyChanged(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
    }

    public void InitializeWithData(Unit owner, UnitEnergyData energyData)
    {
        _owner = owner;
        _playerEnergyData = energyData;
    }
}
