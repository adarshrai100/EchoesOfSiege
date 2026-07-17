using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject _panel;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI _wavesText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _killsText;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    public void Show()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();

        _wavesText.text =
            $"Waves Completed: {waveManager.CurrentWave}";

        _goldText.text =
            $"Gold Earned: {GameStats.Instance.GoldEarned}";

        _killsText.text =
            $"Enemies Defeated: {GameStats.Instance.EnemiesDefeated}";

        _panel.SetActive(true);
    }
}