using UnityEngine;

public class TowerVisualController : MonoBehaviour
{
    [Header("Tower Models")]
    [SerializeField] private GameObject[] _levelModels;

    [Header("Projectile Spawn Points")]
    [SerializeField] private Transform[] _projectileSpawnPoints;

    private Transform _visualRoot;

    public Transform VisualRoot => _visualRoot;
    public Transform CurrentProjectileSpawn { get; private set; }

    private int _currentLevel;

    private void Awake()
    {
        _visualRoot = transform.Find("Visual");
        SetLevel(0);
    }

    public void SetLevel(int level)
    {
        _currentLevel = Mathf.Clamp(level, 0, _levelModels.Length - 1);

        for (int i = 0; i < _levelModels.Length; i++)
        {
            _levelModels[i].SetActive(i == _currentLevel);
        }

        if (_projectileSpawnPoints.Length > _currentLevel)
        {
            CurrentProjectileSpawn = _projectileSpawnPoints[_currentLevel];
        }
    }

    public void UpgradeVisual()
    {
        SetLevel(_currentLevel + 1);
    }

    public void RotateTowards(Vector3 worldPosition, float rotationSpeed)
    {
        if (_visualRoot == null)
            return;

        Vector3 direction = worldPosition - _visualRoot.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _visualRoot.rotation = Quaternion.Slerp(
            _visualRoot.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }
}