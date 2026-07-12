using System.Collections;
using TMPro;
using UnityEngine;

public class WaveBannerUI : MonoBehaviour
{
    public static WaveBannerUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _waveText;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        Instance = this;

        _canvasGroup = _panel.GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
            _canvasGroup = _panel.AddComponent<CanvasGroup>();

        _panel.SetActive(false);
    }

    public void ShowWave(int wave)
    {
        StopAllCoroutines();
        StartCoroutine(PlayBanner(wave));
    }

    private IEnumerator PlayBanner(int wave)
    {
        _panel.SetActive(true);

        _waveText.text = $"WAVE {wave}";

        RectTransform rect = _panel.GetComponent<RectTransform>();

        rect.localScale = Vector3.one * 0.7f;
        _canvasGroup.alpha = 0;

        float duration = 0.25f;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            rect.localScale = Vector3.Lerp(Vector3.one * 0.7f, Vector3.one, t);
            _canvasGroup.alpha = t;

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            _canvasGroup.alpha = 1 - timer / duration;

            yield return null;
        }

        _panel.SetActive(false);
    }
}