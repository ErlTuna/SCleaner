using UnityEngine;

public class LevelEnemyTracker : MonoBehaviour
{
    [SerializeField] IntEventChannelSO _enemiesSpawnedEventChannel;
    [SerializeField] VoidEventChannelSO _enemyDefeatedEventChannel;
    [SerializeField] bool _overrideWinCondition = false;
    [SerializeField] int _aliveEnemies;

    void OnEnable()
    {
        Debug.Log("Level enemy count enabled.");
        _enemiesSpawnedEventChannel.OnEventRaised += SetEnemyCount;
        _enemyDefeatedEventChannel.OnEventRaised += OnEnemyDefeat;
    }
    
    void OnDisable()
    {
       _enemiesSpawnedEventChannel.OnEventRaised -= SetEnemyCount; 
       _enemyDefeatedEventChannel.OnEventRaised -= OnEnemyDefeat;
    }

    void SetEnemyCount(int enemyCount)
    {
        _aliveEnemies = enemyCount;
    }

    void OnEnemyDefeat()
    {
        _aliveEnemies--;

        if (_aliveEnemies <= 0)
        {
            _aliveEnemies = 0;
            OnAllEnemiesDefeated();
        }
    }

    void OnAllEnemiesDefeated()
    {
        Debug.Log("Level complete!");
        GameManager.Instance.SetGameState(GameState.LEVEL_COMPLETED);
    }

}
