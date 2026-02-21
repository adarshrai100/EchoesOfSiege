using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _baseDamage = 10f;

    private PathManager _pathManager;
    private int _currentWaypointIndex;
    private BaseHealth _baseHealth;

    public int CurrentWaypointIndex => _currentWaypointIndex;

    public void Initialize(PathManager pathManager, BaseHealth baseHealth)
    {
        _pathManager = pathManager;
        _baseHealth = baseHealth;
        _currentWaypointIndex = 0;

        transform.position = _pathManager.GetStartWaypoint().position;
    }

    private void Update()
    {
        if (_pathManager == null) return;

        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        if (_currentWaypointIndex >= _pathManager.WaypointCount)
            return;

        Transform targetWaypoint = _pathManager.GetWaypoint(_currentWaypointIndex);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            _moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _pathManager.WaypointCount)
            {
                ReachBase();
            }
        }
    }

    private void ReachBase()
    {
        if (_baseHealth == null) return;

        Debug.Log("Enemy reached base and dealt damage.");
        _baseHealth.TakeDamage(_baseDamage);

        Destroy(gameObject);
    }
}