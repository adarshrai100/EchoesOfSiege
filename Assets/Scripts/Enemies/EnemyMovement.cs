using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _baseDamage = 10f;

    private PathManager _pathManager;
    private BaseHealth _baseHealth;
    private ObjectPool _enemyPool;

    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private int _currentWaypointIndex;

    public int CurrentWaypointIndex => _currentWaypointIndex;


    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void Initialize(PathManager pathManager, BaseHealth baseHealth, ObjectPool enemyPool)
    {
        _pathManager = pathManager;
        _baseHealth = baseHealth;
        _enemyPool = enemyPool;

        _currentWaypointIndex = 0;

        // Reset position to start waypoint
        transform.position = _pathManager.GetStartWaypoint().position;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;
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
        _baseHealth?.TakeDamage(_baseDamage);

        if (_enemyPool != null)
        {
            _enemyPool.Return(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetColor(Color color)
    {
        if (_renderer == null) return;

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_BaseColor", color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}