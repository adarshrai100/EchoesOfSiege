using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private ObjectPool _basicEnemyPool;
    [SerializeField] private ObjectPool _fastEnemyPool;
    [SerializeField] private ObjectPool _tankEnemyPool;

    [Header("Wave Timing")]
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _timeBetweenWaves = 5f;

    [Header("Wave Scaling")]
    [SerializeField] private int _baseEnemiesPerWave = 5;
    [SerializeField] private float _healthGrowthPerWave = 2f;
    [SerializeField] private float _speedGrowthPerWave = 0.2f;

    [Header("Game Settings")]
    [SerializeField] private int _maxWaves = 10;

    [Header("Base Enemy Stats")]
    [SerializeField] private float _basicHealth = 10f;
    [SerializeField] private float _basicSpeed = 3f;

    [SerializeField] private float _fastHealth = 5f;
    [SerializeField] private float _fastSpeed = 6f;

    [SerializeField] private float _tankHealth = 30f;
    [SerializeField] private float _tankSpeed = 1.5f;

    private int _currentWave = 0;
    public int CurrentWave => _currentWave;
    private int _aliveEnemies = 0;

    private void Start()
    {
        StartCoroutine(StartWaveLoop());
    }

    public void RegisterEnemySpawn()
    {
        _aliveEnemies++;
        Debug.Log($"Alive Enemies: {_aliveEnemies}");
    }

    public void RegisterEnemyDespawn()
    {
        _aliveEnemies--;
        Debug.Log($"Alive Enemies: {_aliveEnemies}");
    }

    private IEnumerator StartWaveLoop()
    {
        while (!GameManager.Instance.IsGameOver &&
       _currentWave < _maxWaves)
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            if (GameManager.Instance.IsGameOver)
                yield break;

            _currentWave++;
            AudioManager.Instance?.PlayWaveStart();
            // Show the wave banner
            WaveBannerUI.Instance?.ShowWave(_currentWave);

            // Give the player time to read it
            yield return new WaitForSeconds(1.5f);

            // Spawn enemies
            yield return StartCoroutine(SpawnWave());
        }

        // All waves have been spawned.
        // Wait until every remaining enemy is gone.
        while (_aliveEnemies > 0 && !GameManager.Instance.IsGameOver)
        {
            yield return null;
        }

        if (!GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.TriggerVictory();
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
        EnemyType type = GetRandomEnemyType();

        ObjectPool pool = null;

        switch (type)
        {
            case EnemyType.Basic:
                pool = _basicEnemyPool;
                break;

            case EnemyType.Fast:
                pool = _fastEnemyPool;
                break;

            case EnemyType.Tank:
                pool = _tankEnemyPool;
                break;
        }

        GameObject obj = pool.Get();

        RegisterEnemySpawn();

        EnemyMovement movement = obj.GetComponent<EnemyMovement>();
        EnemyHealth health = obj.GetComponent<EnemyHealth>();

        ConfigureEnemy(type, movement, health);

        movement.Initialize(_pathManager, _baseHealth, pool);
        health.Initialize(pool);
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