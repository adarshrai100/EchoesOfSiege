using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private int _reward = 10;

    private float _currentHealth;

    private ResourceManager _resourceManager;
    private ObjectPool _enemyPool;

    private Vector3 _originalScale;
    private Renderer _renderer;
    private Material _material;
    private Color _originalColor;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = _originalScale;
        if (_material != null)
            _material.color = _originalColor;
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
            return;
        }

        StartCoroutine(HitFlash());
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

    private System.Collections.IEnumerator HitFlash()
    {
        float duration = 0.05f;
        float timer = 0f;

        Vector3 shrinkScale = _originalScale * 0.9f;
        Vector3 overshootScale = _originalScale * 1.05f;

        // Quick shrink
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(_originalScale, shrinkScale, t);
            yield return null;
        }

        timer = 0f;

        // Quick overshoot expand
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(shrinkScale, overshootScale, t);
            yield return null;
        }

        timer = 0f;

        // Return to normal
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(overshootScale, _originalScale, t);
            yield return null;
        }

        transform.localScale = _originalScale;
    }
}