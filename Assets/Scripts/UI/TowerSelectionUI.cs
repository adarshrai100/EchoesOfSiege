using TMPro;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    public static TowerSelectionUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _sellValueText;
    [SerializeField] private ResourceManager _resourceManager;

    private TowerBase _selectedTower;

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
        }
    }

    public void OnSellClicked()
    {
        if (_selectedTower == null) return;

        int refund = _selectedTower.GetSellValue();
        _resourceManager.Add(refund);

        _selectedTower.Sell();
        DeselectTower();
    }

    private void RefreshUI()
    {
        if (_selectedTower == null) return;

        _levelText.text = $"Level: {_selectedTower.CurrentLevel}";

        _costText.text = _selectedTower.CanUpgrade
            ? $"Upgrade Cost: {_selectedTower.CurrentUpgradeCost}"
            : "Max Level";

        _sellValueText.text = $"Sell: {_selectedTower.GetSellValue()}";
    }
}