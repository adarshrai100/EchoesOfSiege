using TMPro;
using UnityEngine;
using System.Collections;

public class TowerSelectionUI : MonoBehaviour
{
    public static TowerSelectionUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _sellValueText;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    [SerializeField] private TextMeshProUGUI _fireRateText;

    private TowerBase _selectedTower;
    private Coroutine _panelAnimation;

    public bool IsPanelOpen => _panel.activeSelf;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    public void SelectTower(TowerBase tower)
    {
        if (_selectedTower != null)
            _selectedTower.SetSelected(false);

        _selectedTower = tower;
        _selectedTower.SetSelected(true);

        _panel.SetActive(true);

        if (_panelAnimation != null)
            StopCoroutine(_panelAnimation);

        _panelAnimation = StartCoroutine(PlayPanelAnimation());

        RefreshUI();
    }

    public void DeselectTower()
    {
        if (_selectedTower != null)
            _selectedTower.SetSelected(false);

        _selectedTower = null;
        _panel.SetActive(false);
    }

    public void OnUpgradeClicked()
    {
        if (_selectedTower == null) return;
        if (!_selectedTower.CanUpgrade) return;

        int cost = _selectedTower.CurrentUpgradeCost;

        if (_resourceManager.CanAfford(cost))
        {
            _resourceManager.Spend(cost);
            _selectedTower.Upgrade();
            RefreshUI();
            AudioManager.Instance?.PlayUpgrade();
        }
    }

    public void OnSellClicked()
    {
        if (_selectedTower == null) return;

        int refund = _selectedTower.GetSellValue();
        _resourceManager.Add(refund);

        _selectedTower.Sell();
        DeselectTower();
        AudioManager.Instance?.PlaySell();
    }

    private void RefreshUI()
    {
        Debug.Log($"Selected Tower: {_selectedTower}");
        Debug.Log($"LevelText: {_levelText}");
        Debug.Log($"CostText: {_costText}");
        Debug.Log($"SellText: {_sellValueText}");
        Debug.Log($"DamageText: {_damageText}");
        Debug.Log($"RangeText: {_rangeText}");
        Debug.Log($"FireRateText: {_fireRateText}");

        if (_selectedTower == null) return;

        _levelText.text = $"Level: {_selectedTower.CurrentLevel}";

        _costText.text = _selectedTower.CanUpgrade
            ? $"Upgrade Cost: {_selectedTower.CurrentUpgradeCost}"
            : "Max Level";

        _sellValueText.text = $"Sell: {_selectedTower.GetSellValue()}";
        _damageText.text = $"Damage: {_selectedTower.Damage}";
        _rangeText.text = $"Range: {_selectedTower.Range}";
        _fireRateText.text = $"Fire Rate: {_selectedTower.FireRate:F2}";
    }

    private IEnumerator PlayPanelAnimation()
    {
        RectTransform rect = _panel.GetComponent<RectTransform>();

        Vector3 startScale = Vector3.one * 0.9f;
        Vector3 overshootScale = Vector3.one * 1.05f;
        Vector3 finalScale = Vector3.one;

        rect.localScale = startScale;

        float duration = 0.08f;
        float timer = 0f;

        // Scale up
        while (timer < duration)
        {
            timer += Time.deltaTime;

            rect.localScale = Vector3.Lerp(
                startScale,
                overshootScale,
                timer / duration);

            yield return null;
        }

        timer = 0f;

        // Settle back
        while (timer < duration)
        {
            timer += Time.deltaTime;

            rect.localScale = Vector3.Lerp(
                overshootScale,
                finalScale,
                timer / duration);

            yield return null;
        }

        rect.localScale = finalScale;
    }
}