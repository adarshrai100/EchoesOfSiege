using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [SerializeField] private FloatingRewardText _rewardTextPrefab;
    [SerializeField] private RectTransform _floatingTextParent;

    private Camera _mainCamera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _mainCamera = Camera.main;
    }

    public void ShowReward(Vector3 worldPosition, int amount)
    {
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);

        FloatingRewardText reward =
            Instantiate(_rewardTextPrefab, _floatingTextParent);

        reward.SetPosition(screenPosition);

        reward.Initialize(amount);
    }
}