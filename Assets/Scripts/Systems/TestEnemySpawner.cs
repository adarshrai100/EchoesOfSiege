using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyPrefab;
    [SerializeField] private PathManager _pathManager;

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        EnemyMovement enemy = Instantiate(_enemyPrefab);
        enemy.Initialize(_pathManager);
    }
}