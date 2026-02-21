using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private EnemyHealth _target;
    private float _damage;

    public void Initialize(EnemyHealth target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _target.transform.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (_target != null)
        {
            _target.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}