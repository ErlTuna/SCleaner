using System;
using System.Collections.Generic;
using UnityEngine;
public class UI_HealthbarController : MonoBehaviour
{
    [SerializeField] PlayerHealthInitializedEventChannel _playerHealthInitializedEventChannel;
    [SerializeField] PlayerMaxHealthChangedEventChannel _playerMaxHealthChangedEventChannel;
    [SerializeField] PlayerHealthChangedEventChannel _playerHealthChangedEventChannel;
    [SerializeField] PlayerShieldHealthChangedEventChannel _playerShieldHealthChangedEventChannel;
    [SerializeField] GameObject _healthContainerParent;
    [SerializeField] GameObject _shieldContainerParent;
    [SerializeField] GameObject _shieldContainerPrefab;
    [SerializeField] GameObject _healthContainerPrefab;
    [SerializeField] GameObject _shieldHealthContainerPrefab;
    readonly List<UI_HealthbarContainer> _HPContainers = new();
    readonly List<UI_HealthbarContainer> _shieldHPContainers = new();
    int _currentHPContainerIndex = 0;
    UI_HealthbarContainer CurrentHealthContainer
    {
        get
        {
            if (_HPContainers == null || _HPContainers.Count == 0)
                return null;

            int index = Mathf.Clamp(_currentHPContainerIndex, 0, _HPContainers.Count - 1);
            return _HPContainers[index];
        }
    }

    UI_HealthbarContainer CurrentShieldContainer => _shieldHPContainers.Count > 0 ? _shieldHPContainers[^1] : null;
    [SerializeField] int _containerSize = 2;


    void OnEnable()
    {
        _playerHealthInitializedEventChannel.OnEventRaised += InitializeHealthBar;
        _playerHealthChangedEventChannel.OnEventRaised += UpdateHealthContainers;
        _playerShieldHealthChangedEventChannel.OnEventRaised += UpdateShieldContainers;
        _playerMaxHealthChangedEventChannel.OnEventRaised += UpdateHealthContainerCount;
    }

    void OnDisable()
    {
        _playerHealthInitializedEventChannel.OnEventRaised -= InitializeHealthBar;
        _playerHealthChangedEventChannel.OnEventRaised -= UpdateHealthContainers;
        _playerShieldHealthChangedEventChannel.OnEventRaised -= UpdateShieldContainers;
        _playerMaxHealthChangedEventChannel.OnEventRaised -= UpdateHealthContainerCount;
    }

    void InitializeHealthBar(UnitHealthData healthData)
    {
        Debug.Log("Initializing healthbar");
        int healthContainerCount = (healthData.MaxHealth.Value + 1) / _containerSize;
        // Health containers
        for (int i = 0; i < healthContainerCount; ++i)
        {
            GameObject go = Instantiate(_healthContainerPrefab, _healthContainerParent.transform);
            go.name = $"HealthContainer {i}";
            var container = go.GetComponent<UI_HealthbarContainer>();
            _HPContainers.Add(container);
        }

        

        int fullHealthContainers = healthData.CurrentHealth / 2;
        int halfHealthContainers = healthData.CurrentHealth % 2;

        for (int i = 0; i < fullHealthContainers; ++i)
        {
            _HPContainers[i].UpdateContainer(HealthbarContainerState.FULL);
        }

        _currentHPContainerIndex = fullHealthContainers - 1;

        Debug.Log("Current HP Container Index after init : " + _currentHPContainerIndex);

        if (halfHealthContainers > 0)
        {
            ++_currentHPContainerIndex;
            CurrentHealthContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
        }
        
        
        
        // Shield containers
        int fullShieldContainers = healthData.CurrentShieldHealth / 2;
        int halfShieldContainer = healthData.CurrentShieldHealth % 2;
        int shieldcontainerCount = fullShieldContainers + halfShieldContainer;


        for (int i = 1; i <= shieldcontainerCount; ++i)
        {
            GameObject shieldContainer = Instantiate(_shieldHealthContainerPrefab, _shieldContainerParent.transform);
            shieldContainer.name = $"Shield Container {i}";
            UI_HealthbarContainer containerScript = shieldContainer.GetComponent<UI_HealthbarContainer>();
            _shieldHPContainers.Add(containerScript);
        }

        Debug.Log("Created shield containers : " + shieldcontainerCount);

        for (int i = 0; i < fullShieldContainers; ++i)
        {
            _shieldHPContainers[i].UpdateContainer(HealthbarContainerState.FULL);
        }

        if (halfShieldContainer > 0)
        {
            CurrentShieldContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
        }
    }

    void UpdateHealthContainerCount(StatChangedArgs maxHealthChangeArgs)
    {   
        if (maxHealthChangeArgs.Delta > 0)
        {
            int healthContainersToAdd = maxHealthChangeArgs.Delta / _containerSize;
            for (int i = 0; i < healthContainersToAdd; ++i)
            {
                AddHealthContainer(HealthbarContainerState.DEPLETED);
            }
        }

        else if (maxHealthChangeArgs.Delta < 0)
        {
            int healthContainersToRemove = Mathf.Abs(maxHealthChangeArgs.Delta / _containerSize);
            for (int i = 0; i < healthContainersToRemove; ++i)
            {
                if (_HPContainers.Count == 0)
                    break;

                RemoveHealthContainer();
            }
        }
    }

    

    void UpdateHealthContainers(StatChangedArgs healthChangedArgs)
    {
        Debug.Log("Delta value of health changed args : " + healthChangedArgs.Delta);
        if (healthChangedArgs.Delta > 0)
        {
            int amountToFill = healthChangedArgs.Delta;

            while (amountToFill > 0)
            {
                if (CurrentHealthContainer.ContainerState == HealthbarContainerState.DEPLETED)
                {
                    CurrentHealthContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
                    --amountToFill;
                }


                else if (CurrentHealthContainer.ContainerState == HealthbarContainerState.HALF_FULL)
                {
                    CurrentHealthContainer.UpdateContainer(HealthbarContainerState.FULL);
                    --amountToFill;
                }

                //(CurrentHealthContainer.ContainerState == HealthbarContainerState.FULL)
                else
                {
                    if (_currentHPContainerIndex != _HPContainers.Count - 1)
                    {
                        ++_currentHPContainerIndex;
                        CurrentHealthContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
                        amountToFill -= _containerSize;
                    }
                }
            }
            
        }

        else if (healthChangedArgs.Delta < 0)
        {
            int amountToTakeAway = Mathf.Abs(healthChangedArgs.Delta);
            while (amountToTakeAway > 0)
            {
                if (CurrentHealthContainer.ContainerState == HealthbarContainerState.FULL)
                {
                    CurrentHealthContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
                }

                else if (CurrentHealthContainer.ContainerState == HealthbarContainerState.HALF_FULL)
                {
                    CurrentHealthContainer.UpdateContainer(HealthbarContainerState.DEPLETED);
                    if (_currentHPContainerIndex > 0)
                        --_currentHPContainerIndex;
                }

                --amountToTakeAway;
            }
        }


    }

    void UpdateShieldContainers(StatChangedArgs shieldHealthArgs)
    {
        Debug.Log("Delta passed : " + shieldHealthArgs.Delta);


        if (shieldHealthArgs.Delta > 0)
        {
            int amountToFill = shieldHealthArgs.Delta;

            if (CurrentShieldContainer != null && CurrentShieldContainer.ContainerState == HealthbarContainerState.HALF_FULL)
            {
                CurrentShieldContainer.UpdateContainer(HealthbarContainerState.FULL);
                amountToFill -= 1;
            }

            int shieldContainersToAdd = amountToFill / _containerSize;
            for (int i = 0; i < shieldContainersToAdd; ++i)
            {
                AddShieldContainer(HealthbarContainerState.FULL);
                amountToFill -= _containerSize;
            }

            if (amountToFill > 0)
            {
                AddShieldContainer(HealthbarContainerState.HALF_FULL);
            }
        }


        else if (shieldHealthArgs.Delta < 0)
        {
            int amountToRemove = Mathf.Abs(shieldHealthArgs.Delta);
            if (CurrentShieldContainer != null && CurrentShieldContainer.ContainerState == HealthbarContainerState.HALF_FULL)
            {
                RemoveCurrentShieldContainer();
                amountToRemove -= 1;
            }

            int shieldContainersToRemove = amountToRemove / _containerSize;
            for (int i = 0; i < shieldContainersToRemove; ++i)
            {
                if (_shieldHPContainers.Count == 0)
                    break;

                RemoveCurrentShieldContainer();
                amountToRemove -= _containerSize;
            }

            if ( (amountToRemove > 0) && CurrentShieldContainer)
                CurrentShieldContainer.UpdateContainer(HealthbarContainerState.HALF_FULL);
        }
    }

    void AddHealthContainer(HealthbarContainerState state)
    {
        GameObject go = Instantiate(_healthContainerPrefab, _healthContainerParent.transform);
        var container = go.GetComponent<UI_HealthbarContainer>();
        container.UpdateContainer(state);
        _HPContainers.Add(container);
    }

    void RemoveHealthContainer()
    {
        UI_HealthbarContainer lastContainer = _HPContainers[^1];
        Destroy(lastContainer.gameObject);
        _HPContainers.RemoveAt(_HPContainers.Count - 1);        
    }

    void AddShieldContainer(HealthbarContainerState state)
    {
        GameObject go = Instantiate(_shieldHealthContainerPrefab, _shieldContainerParent.transform);
        var container = go.GetComponent<UI_HealthbarContainer>();
        container.UpdateContainer(state);
        _shieldHPContainers.Add(container);
    }

    void RemoveCurrentShieldContainer()
    {
        Debug.Log("Is CurrentShieldContainer valid?" + CurrentShieldContainer);
        Destroy(CurrentShieldContainer.gameObject);
        _shieldHPContainers.RemoveAt(_shieldHPContainers.Count - 1);
    }


}