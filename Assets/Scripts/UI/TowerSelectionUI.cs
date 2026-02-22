using TMPro;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    public static TowerSelectionUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private ResourceManager _resourceManager;

    private TowerBase _selectedTower;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    public void SelectTower(TowerBase tower)
    {
        _selectedTower = tower;
        _panel.SetActive(true);
        RefreshUI();
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

    private void RefreshUI()
    {
        if (_selectedTower == null) return;

        _levelText.text = $"Level: {_selectedTower.CurrentLevel}";
        _costText.text = _selectedTower.CanUpgrade
            ? $"Upgrade Cost: {_selectedTower.CurrentUpgradeCost}"
            : "Max Level";
    }
}