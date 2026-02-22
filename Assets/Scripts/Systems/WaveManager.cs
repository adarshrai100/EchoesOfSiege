using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private EnemyMovement _enemyPrefab;
    [SerializeField] private PathManager _pathManager;
    [SerializeField] private int _enemiesPerWave = 5;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _timeBetweenWaves = 5f;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private ObjectPool _enemyPool;

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
        for (int i = 0; i < _enemiesPerWave; i++)
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
        int rand = Random.Range(0, 3);
        return (EnemyType)rand;
    }

    private void ConfigureEnemy(EnemyType type, EnemyMovement movement, EnemyHealth health)
    {
        switch (type)
        {
            case EnemyType.Basic:
                health.SetMaxHealth(_basicHealth);
                movement.SetMoveSpeed(_basicSpeed);
                movement.SetColor(Color.white);
                break;

            case EnemyType.Fast:
                health.SetMaxHealth(_fastHealth);
                movement.SetMoveSpeed(_fastSpeed);
                movement.SetColor(Color.yellow);
                break;

            case EnemyType.Tank:
                health.SetMaxHealth(_tankHealth);
                movement.SetMoveSpeed(_tankSpeed);
                movement.SetColor(Color.red);
                break;
        }
    }
}