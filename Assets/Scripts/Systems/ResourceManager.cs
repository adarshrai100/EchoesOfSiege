using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private int _startingMoney = 100;

    private int _currentMoney;

    public int CurrentMoney => _currentMoney;

    private void Awake()
    {
        _currentMoney = _startingMoney;
        Debug.Log("Starting Money: " + _currentMoney);
    }

    public bool CanAfford(int amount)
    {
        return _currentMoney >= amount;
    }

    public void Spend(int amount)
    {
        _currentMoney -= amount;
        Debug.Log("Money After Spend: " + _currentMoney);
    }

    public void Add(int amount)
    {
        _currentMoney += amount;
        Debug.Log("Money After Reward: " + _currentMoney);
    }
}