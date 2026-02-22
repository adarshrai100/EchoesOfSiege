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

        movement.Initialize(_pathManager, _baseHealth, _enemyPool);
        health.Initialize(_enemyPool);
    }
}