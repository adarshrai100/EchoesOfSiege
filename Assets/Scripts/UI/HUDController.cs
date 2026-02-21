using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _baseHealthText;
    [SerializeField] private TextMeshProUGUI _waveText;

    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private WaveManager _waveManager;

    private void Update()
    {
        _moneyText.text = $"Money: {_resourceManager.CurrentMoney}";
        _baseHealthText.text = $"Base HP: {_baseHealth.CurrentHealth}";
        _waveText.text = $"Wave: {_waveManager.CurrentWave}";
    }
}