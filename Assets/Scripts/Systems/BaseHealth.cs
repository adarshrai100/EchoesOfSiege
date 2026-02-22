using UnityEngine;

public class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;
    private bool _isDead;

    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _isDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0f);

        Debug.Log("Base Health: " + _currentHealth);

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        GameManager.Instance?.TriggerGameOver();
        // Later we will stop waves and show restart UI
    }
}