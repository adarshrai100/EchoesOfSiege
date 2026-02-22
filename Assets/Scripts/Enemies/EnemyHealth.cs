using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private int _reward = 10;

    private ResourceManager _resourceManager;
    private ObjectPool _enemyPool;
    private float _currentHealth;

    private void Awake()
    {
        _resourceManager = FindObjectOfType<ResourceManager>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        Debug.Log("Enemy HP: " + _currentHealth);

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Initialize(ObjectPool pool)
    {
        _enemyPool = pool;
        _currentHealth = _maxHealth;
    }

    private void Die()
    {
        _resourceManager?.Add(_reward);
        _enemyPool.Return(gameObject);
    }
}