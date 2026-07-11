using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private int _reward = 10;
    [SerializeField] private GameObject _deathVFX;

    [SerializeField] private Transform _visualRoot;

    private float _currentHealth;

    private ResourceManager _resourceManager;
    private ObjectPool _enemyPool;

    private Vector3 _originalVisualScale;
    private Renderer _renderer;
    private Material _material;
    private Color _originalColor;
    private bool _isDying;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;
        _originalVisualScale = _visualRoot.localScale;
        _resourceManager = FindFirstObjectByType<ResourceManager>();
    }

    private void OnEnable()
    {
        _isDying = false;

        _visualRoot.localScale = _originalVisualScale;

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
        if (!gameObject.activeInHierarchy || _isDying)
            return;

        _currentHealth -= amount;

        if (_currentHealth <= 0f)
        {
            _isDying = true;
            StartCoroutine(DeathSequence());
            return;
        }

        StartCoroutine(HitFlash());
    }

    private void Die()
    {
        _resourceManager?.Add(_reward);

        if (_deathVFX != null)
        {
            Instantiate(_deathVFX, transform.position, Quaternion.identity);
        }

        _visualRoot.localScale = _originalVisualScale;

        if (_enemyPool != null)
            _enemyPool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }

    public void SetMaxHealth(float health)
    {
        _maxHealth = health;
    }

    private System.Collections.IEnumerator HitFlash()
    {
        float duration = 0.05f;
        float timer = 0f;

        Vector3 shrinkScale = _originalVisualScale * 0.9f;
        Vector3 overshootScale = _originalVisualScale * 1.05f;

        // Quick shrink
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _visualRoot.localScale = Vector3.Lerp(_originalVisualScale, shrinkScale, t);
            yield return null;
        }

        timer = 0f;

        // Quick overshoot expand
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _visualRoot.localScale = Vector3.Lerp(shrinkScale, overshootScale, t);
            yield return null;
        }

        timer = 0f;

        // Return to normal
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _visualRoot.localScale = Vector3.Lerp(overshootScale, _originalVisualScale, t);
            yield return null;
        }

        _visualRoot.localScale = _originalVisualScale;
    }
    private System.Collections.IEnumerator DeathSequence()
    {
        Vector3 startScale = _visualRoot.localScale;

        Vector3 popScale = startScale * 1.15f;
        Vector3 endScale = Vector3.zero;

        float duration = 0.18f;
        float timer = 0f;

        // Pop slightly larger
        while (timer < duration * 0.4f)
        {
            timer += Time.deltaTime;

            _visualRoot.localScale = Vector3.Lerp(
                startScale,
                popScale,
                timer / (duration * 0.4f));

            yield return null;
        }

        timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 0.25f;

        // Shrink while floating upward
        while (timer < duration * 0.6f)
        {
            timer += Time.deltaTime;

            float t = timer / (duration * 0.6f);

            _visualRoot.localScale = Vector3.Lerp(popScale, endScale, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        transform.position = startPos;

        Die();
    }
}