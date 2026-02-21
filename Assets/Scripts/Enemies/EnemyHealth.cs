using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private int _reward = 10;

    private ResourceManager _resourceManager;
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

    private void Die()
    {
        _resourceManager?.Add(_reward);
        Destroy(gameObject);
    }
}