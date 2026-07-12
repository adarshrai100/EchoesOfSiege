using TMPro;
using UnityEngine;

public class FloatingRewardText : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 60f;
    [SerializeField] private float _lifetime = 1f;

    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;

    private float _timer;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Initialize(int amount)
    {
        _text.text = $"+{amount}";
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);

        _timer += Time.deltaTime;

        _canvasGroup.alpha = Mathf.Lerp(1f, 0f, _timer / _lifetime);

        if (_timer >= _lifetime)
            Destroy(gameObject);
    }

    public void SetPosition(Vector3 screenPosition)
    {
        GetComponent<RectTransform>().position = screenPosition;
    }
}