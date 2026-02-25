using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private int _reward = 10;

    private float _currentHealth;

    private ResourceManager _resourceManager;
    private ObjectPool _enemyPool;

    private void Awake()
    {
        _resourceManager = FindObjectOfType<ResourceManager>();
    }

    public void Initialize(ObjectPool pool)
    {
        _enemyPool = pool;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _resourceManager?.Add(_reward);

        if (_enemyPool != null)
        {
            _enemyPool.Return(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMaxHealth(float health)
    {
        _maxHealth = health;
    }
}