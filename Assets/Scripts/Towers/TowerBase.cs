using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Upgrade Settings")]
    [SerializeField] private int _maxLevel = 3;
    [SerializeField] private int _baseUpgradeCost = 30;
    [SerializeField] private float _damageIncreasePerLevel = 5f;
    [SerializeField] private float _fireRateIncreasePerLevel = 0.5f;
    [SerializeField] private float _sellRefundPercent = 0.7f;

    [SerializeField]
    private TowerType _towerType;

    public TowerType TowerType => _towerType;

    private int _currentLevel = 1;
    private int _currentUpgradeCost;
    private int _totalInvested;
    public float Damage => _damage;
    public float Range => _range;
    public float FireRate => _fireRate;

    private ObjectPool _projectilePool;
    private GridCell _gridCell;

    private float _fireCooldown;
    private EnemyHealth _currentTarget;

    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private static readonly Color _normalColor = Color.white;
    private static readonly Color _selectedColor = new Color(0.6f, 1f, 0.6f);

    public int CurrentLevel => _currentLevel;
    public int CurrentUpgradeCost => _currentUpgradeCost;
    public bool CanUpgrade => _currentLevel < _maxLevel;

    private Vector3 _originalVisualScale;
    private TowerVisualController _visualController;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();

        _visualController = GetComponent<TowerVisualController>();

        if (_visualController != null &&
            _visualController.VisualRoot != null)
        {
            _originalVisualScale = _visualController.VisualRoot.localScale;
        }
    }

    private void Start()
    {
        _currentUpgradeCost = _baseUpgradeCost;
    }

    public void Initialize(ObjectPool projectilePool, GridCell cell)
    {
        _projectilePool = projectilePool;
        _gridCell = cell;
        _totalInvested = 0;
    }

    public void RegisterInitialCost(int cost)
    {
        _totalInvested = cost;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        HandleTargeting();

        HandleRotation();

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

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            FireProjectile();
            _fireCooldown = 1f / _fireRate;
        }
    }

    private void FireProjectile()
    {
        if (_projectilePool == null)
            return;

        GameObject obj = _projectilePool.Get();

        Transform spawnPoint = transform;

        if (_visualController != null &&
            _visualController.CurrentProjectileSpawn != null)
        {
            spawnPoint = _visualController.CurrentProjectileSpawn;
        }

        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = spawnPoint.rotation;

        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Initialize(_currentTarget, _damage, _projectilePool);

        AudioManager.Instance?.PlayShoot();
    }

    public void Upgrade()
    {
        if (!CanUpgrade) return;

        _currentLevel++;

        _damage += _damageIncreasePerLevel;
        _fireRate += _fireRateIncreasePerLevel;

        _totalInvested += _currentUpgradeCost;
        _currentUpgradeCost += _baseUpgradeCost;

        _visualController?.PlayUpgradeAnimation(this);

        _visualController?.UpgradeVisual();
    }

    public int GetSellValue()
    {
        return Mathf.RoundToInt(_totalInvested * _sellRefundPercent);
    }

    public void Sell()
    {
        if (_gridCell != null)
            _gridCell.SetOccupied(false);

        Destroy(gameObject);
    }

    public void SetSelected(bool selected)
    {
        if (_renderer == null) return;

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_BaseColor", selected ? _selectedColor : _normalColor);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    

    private void HandleRotation()
    {
        if (_currentTarget == null)
            return;

        _visualController?.RotateTowards(
            _currentTarget.transform.position,
            _rotationSpeed);
    }

}