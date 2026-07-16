using UnityEngine;

public class TowerVisualController : MonoBehaviour
{
    [Header("Tower Models")]
    [SerializeField] private GameObject[] _levelModels;

    [Header("Projectile Spawn Points")]
    [SerializeField] private Transform[] _projectileSpawnPoints;
    [SerializeField] private GameObject _rangeIndicator;
    public GameObject RangeIndicator => _rangeIndicator;

    private Transform _visualRoot;

    public Transform VisualRoot => _visualRoot;
    public Transform CurrentVisual => _levelModels[_currentLevel].transform;
    public Transform CurrentProjectileSpawn { get; private set; }

    private int _currentLevel;
    private Vector3 _originalScale;
    private Vector3 _originalLocalPosition;
    private Vector3 _visualOriginalLocalPosition;



    private void Awake()
    {
        _visualRoot = transform.Find("Visual");
        _visualOriginalLocalPosition = _visualRoot.localPosition;

        _originalScale = CurrentVisual.localScale;
        _originalLocalPosition = CurrentVisual.localPosition;

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
        _originalScale = CurrentVisual.localScale;
        _originalLocalPosition = CurrentVisual.localPosition;
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

    public void PlayUpgradeAnimation(MonoBehaviour owner)
    {
        owner.StartCoroutine(UpgradePunch());
    }

    private System.Collections.IEnumerator UpgradePunch()
    {
        Transform visual = CurrentVisual;

        float upDuration = 0.06f;
        float downDuration = 0.14f;

        Vector3 punchScale = _originalScale * 1.28f;

        float timer = 0f;

        while (timer < upDuration)
        {
            timer += Time.deltaTime;

            float t = timer / upDuration;

            visual.localScale = Vector3.Lerp(
                _originalScale,
                punchScale,
                t);

            yield return null;
        }

        timer = 0f;

        while (timer < downDuration)
        {
            timer += Time.deltaTime;

            float t = timer / downDuration;

            float ease = 1f - Mathf.Pow(1f - t, 3f);

            visual.localScale = Vector3.Lerp(
                punchScale,
                _originalScale,
                ease);

            yield return null;
        }

        visual.localScale = _originalScale;
    }

    public void PlayBuildAnimation(MonoBehaviour owner)
    {
        owner.StartCoroutine(BuildAnimation());
    }

    private System.Collections.IEnumerator BuildAnimation()
    {
        Renderer renderer = CurrentVisual.GetComponentInChildren<Renderer>();

        float buildHeight = renderer != null
            ? renderer.bounds.size.y
            : 2f;

        Vector3 startPos = _visualOriginalLocalPosition + Vector3.down * buildHeight;
        Vector3 overshootPos = _visualOriginalLocalPosition + Vector3.up * 0.12f;

        _visualRoot.localPosition = startPos;

        // Slight squash while underground
        _visualRoot.localScale = Vector3.one * 0.92f;

        float riseDuration = 0.55f;
        float settleDuration = 0.18f;

        float timer = 0f;

        // Rise
        while (timer < riseDuration)
        {
            timer += Time.deltaTime;

            float t = timer / riseDuration;

            // Smooth ease-out
            float ease = 1f - Mathf.Pow(1f - t, 3f);

            _visualRoot.localPosition =
                Vector3.Lerp(startPos, overshootPos, ease);

            yield return null;
        }

        timer = 0f;

        // Settle + unsquash
        while (timer < settleDuration)
        {
            timer += Time.deltaTime;

            float t = timer / settleDuration;

            _visualRoot.localPosition =
                Vector3.Lerp(overshootPos, _visualOriginalLocalPosition, t);

            _visualRoot.localScale =
                Vector3.Lerp(Vector3.one * 0.92f, Vector3.one, t);

            yield return null;
        }

        _visualRoot.localPosition = _visualOriginalLocalPosition;
        _visualRoot.localScale = Vector3.one;
    }
}