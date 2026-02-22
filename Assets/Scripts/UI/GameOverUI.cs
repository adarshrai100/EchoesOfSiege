using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            _panel.SetActive(true);
        }
    }

    public void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }
}