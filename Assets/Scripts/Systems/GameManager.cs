using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f); // Match your fade duration

        AudioManager.Instance.PlayGameplayMusic();
    }

    public void TriggerGameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        Debug.Log("Game Over Triggered");

        AudioManager.Instance?.PlayGameOver();
        AudioManager.Instance?.PlayGameOverMusic();

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


        AudioManager.Instance?.PlayVictoryMusic();

        VictoryUI.Instance?.Show();

        Time.timeScale = 0f;
    }
}