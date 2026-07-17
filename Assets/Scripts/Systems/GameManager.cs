using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameOver { get; private set; }
    public bool IsVictory { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        IsGameOver = false;
        IsVictory = false;
    }

    private void Start()
    {
        AudioManager.Instance?.PlayGameplayMusic();
    }

    public void TriggerGameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("Game Over Triggered");
 

        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlayGameOver();

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerVictory()
    {
        if (IsGameOver || IsVictory)
            return;

        IsVictory = true;

        Debug.Log("Victory!");

        AudioManager.Instance?.StopMusic();

        VictoryUI.Instance?.Show();

        Time.timeScale = 0f;
    }
}