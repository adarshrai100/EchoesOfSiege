using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _damage = 10f;

    private ObjectPool _projectilePool;

    private float _fireCooldown;
    private EnemyHealth _currentTarget;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;
        HandleTargeting();
        HandleAttack();
    }

    public void Initialize(ObjectPool projectilePool)
    {
        _projectilePool = projectilePool;
    }

    private void HandleTargeting()
    {
        // Keep target if still valid
        if (_currentTarget != null && _currentTarget.gameObject.activeInHierarchy)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _range);

        EnemyHealth bestTarget = null;
        int highestProgress = -1;

        foreach (Collider hit in hits)
        {
            EnemyMovement movement = hit.GetComponent<EnemyMovement>();
            EnemyHealth health = hit.GetComponent<EnemyHealth>();

            if (movement != null && health != null)
            {
                if (movement.CurrentWaypointIndex > highestProgress)
                {
                    highestProgress = movement.CurrentWaypointIndex;
                    bestTarget = health;
                }
            }
        }

        _currentTarget = bestTarget;
    }

    private void HandleAttack()
    {
        if (_currentTarget == null) return;

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            FireProjectile();
            _fireCooldown = 1f / _fireRate;
        }
    }

    private void FireProjectile()
    {
        if (_projectilePool == null) return;

        GameObject obj = _projectilePool.Get();
        obj.transform.position = transform.position;

        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Initialize(_currentTarget, _damage, _projectilePool);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}