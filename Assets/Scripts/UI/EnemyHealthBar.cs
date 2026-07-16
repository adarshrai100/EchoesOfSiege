using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void SetHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}