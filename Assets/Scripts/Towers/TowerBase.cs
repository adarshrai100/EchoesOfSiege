using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private Projectile _projectilePrefab;

    private float _fireCooldown;
    private EnemyHealth _currentTarget;

    private void Update()
    {
        HandleTargeting();
        HandleAttack();
    }

    private void HandleTargeting()
    {
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

        if (_currentTarget == null || !_currentTarget.gameObject.activeInHierarchy)
        {
            _currentTarget = null;
            return;
        }

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            Projectile projectile = Instantiate(
            _projectilePrefab,
            transform.position,
            Quaternion.identity
            );

            projectile.Initialize(_currentTarget, _damage);
            _fireCooldown = 1f / _fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}