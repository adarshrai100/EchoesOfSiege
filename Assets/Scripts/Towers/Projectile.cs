using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private EnemyHealth _target;
    private float _damage;
    private ObjectPool _pool;

    public void Initialize(EnemyHealth target, float damage, ObjectPool pool)
    {
        _target = target;
        _damage = damage;
        _pool = pool;
    }

    private void Update()
    {
        if (_target == null)
        {
            _pool.Return(gameObject);
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

        _pool.Return(gameObject);
    }
}