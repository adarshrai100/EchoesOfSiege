using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _damage = 5f;

    private float _fireCooldown;
    private EnemyHealth _currentTarget;

    private void Update()
    {
        HandleTargeting();
        HandleAttack();
    }

    private void HandleTargeting()
    {
        if (_currentTarget != null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _range);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                _currentTarget = enemy;
                break;
            }
        }
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
            _currentTarget.TakeDamage(_damage);
            _fireCooldown = 1f / _fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}