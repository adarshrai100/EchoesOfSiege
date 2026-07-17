using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int EnemiesDefeated { get; private set; }
    public int GoldEarned { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddEnemyKill()
    {
        EnemiesDefeated++;
    }

    public void AddGold(int amount)
    {
        GoldEarned += amount;
    }

    public void ResetStats()
    {
        EnemiesDefeated = 0;
        GoldEarned = 0;
    }
}