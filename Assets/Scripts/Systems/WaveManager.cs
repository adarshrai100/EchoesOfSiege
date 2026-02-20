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

    private int _currentWave = 0;

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
        EnemyMovement enemy = Instantiate(_enemyPrefab);
        enemy.Initialize(_pathManager);
    }
}