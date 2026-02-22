using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private ObjectPool _enemyPool;

    [Header("Wave Timing")]
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _timeBetweenWaves = 5f;

    [Header("Wave Scaling")]
    [SerializeField] private int _baseEnemiesPerWave = 5;
    [SerializeField] private float _healthGrowthPerWave = 2f;
    [SerializeField] private float _speedGrowthPerWave = 0.2f;

    [Header("Base Enemy Stats")]
    [SerializeField] private float _basicHealth = 10f;
    [SerializeField] private float _basicSpeed = 3f;

    [SerializeField] private float _fastHealth = 5f;
    [SerializeField] private float _fastSpeed = 6f;

    [SerializeField] private float _tankHealth = 30f;
    [SerializeField] private float _tankSpeed = 1.5f;

    private int _currentWave = 0;
    public int CurrentWave => _currentWave;

    private void Start()
    {
        StartCoroutine(StartWaveLoop());
    }

    private IEnumerator StartWaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            _currentWave++;
            Debug.Log("Starting Wave: " + _currentWave);

            yield return StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        int enemiesThisWave = _baseEnemiesPerWave + (_currentWave - 1);

        for (int i = 0; i < enemiesThisWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        GameObject obj = _enemyPool.Get();

        EnemyMovement movement = obj.GetComponent<EnemyMovement>();
        EnemyHealth health = obj.GetComponent<EnemyHealth>();

        EnemyType type = GetRandomEnemyType();

        ConfigureEnemy(type, movement, health);

        movement.Initialize(_pathManager, _baseHealth, _enemyPool);
        health.Initialize(_enemyPool);
    }

    private EnemyType GetRandomEnemyType()
    {
        float tankChance = Mathf.Clamp01(_currentWave * 0.05f);
        float roll = Random.value;

        if (roll < tankChance)
            return EnemyType.Tank;

        if (roll < 0.5f)
            return EnemyType.Fast;

        return EnemyType.Basic;
    }

    private void ConfigureEnemy(EnemyType type, EnemyMovement movement, EnemyHealth health)
    {
        float scaledHealth;
        float scaledSpeed;

        switch (type)
        {
            case EnemyType.Basic:
                scaledHealth = _basicHealth + (_currentWave * _healthGrowthPerWave);
                scaledSpeed = _basicSpeed + (_currentWave * _speedGrowthPerWave);

                health.SetMaxHealth(scaledHealth);
                movement.SetMoveSpeed(scaledSpeed);
                movement.SetColor(Color.white);
                break;

            case EnemyType.Fast:
                scaledHealth = _fastHealth + (_currentWave * _healthGrowthPerWave);
                scaledSpeed = _fastSpeed + (_currentWave * _speedGrowthPerWave);

                health.SetMaxHealth(scaledHealth);
                movement.SetMoveSpeed(scaledSpeed);
                movement.SetColor(Color.yellow);
                break;

            case EnemyType.Tank:
                scaledHealth = _tankHealth + (_currentWave * _healthGrowthPerWave);
                scaledSpeed = _tankSpeed + (_currentWave * _speedGrowthPerWave);

                health.SetMaxHealth(scaledHealth);
                movement.SetMoveSpeed(scaledSpeed);
                movement.SetColor(Color.red);
                break;
        }
    }
}