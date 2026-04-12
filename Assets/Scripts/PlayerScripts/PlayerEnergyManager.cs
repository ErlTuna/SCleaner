using System.Collections;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds_1 = new(.1f);
    Unit _owner;
    [SerializeField] FloatFloatEventChannel _playerEnergyChangedEventChannel;
    [SerializeField] PlayerEnergyConfigSO _playerEnergyConfig;
    [SerializeField] PlayerEnergyData _playerEnergyData;
    Coroutine _rechargeCoroutine;
    bool _isRecharging = false;

    public void InitializeManager(PlayerEnergyData playerEnergyData, PlayerEnergyConfigSO playerEnergyConfig)
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
        _rechargeCoroutine = StartCoroutine(RechargeOvertime());

    }

    public IEnumerator RechargeOvertime()
    {
        while (_playerEnergyData.CurrentEnergy < _playerEnergyData.MaxEnergy)
        {
            _isRecharging = true;
            _playerEnergyData.CurrentEnergy += _playerEnergyData.RechargeRate;

            if (_playerEnergyData.CurrentEnergy >= _playerEnergyData.MaxEnergy)
            {
                _playerEnergyData.CurrentEnergy = _playerEnergyData.MaxEnergy;

                _playerEnergyChangedEventChannel.RaiseEvent(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
                AudioSource.PlayClipAtPoint(_playerEnergyConfig.fullEnergySFX, gameObject.transform.position);
                _isRecharging = false;
                yield break;
            }

            _playerEnergyChangedEventChannel.RaiseEvent(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
            yield return _waitForSeconds_1;
        }

        _isRecharging = false;
        _rechargeCoroutine = null;
    }

    public bool CanRecharge()
    {
        return _playerEnergyData.CurrentEnergy != _playerEnergyData.MaxEnergy && !_isRecharging;
    }

    public void OnEnergyUse(AbilityData abilityData)
    {
        float energyAfterUse = _playerEnergyData.CurrentEnergy - abilityData.EnergyCost;
        if (Mathf.Approximately(energyAfterUse, 0))
        {
            _playerEnergyData.CurrentEnergy = 0;
            _playerEnergyChangedEventChannel.RaiseEvent(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
            return;
        }

        _playerEnergyData.CurrentEnergy -= abilityData.EnergyCost;
        _playerEnergyChangedEventChannel.RaiseEvent(_playerEnergyData.CurrentEnergy, _playerEnergyData.MaxEnergy);
    }

    public void InitializeWithData(Unit owner, PlayerEnergyData energyData)
    {
        _owner = owner;
        _playerEnergyData = energyData;
    }
}
