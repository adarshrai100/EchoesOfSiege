using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private bool _isTransitioning;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        if (_isTransitioning)
            return;

        _isTransitioning = true;
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;

        color.a = 1f;
        fadeImage.color = color;

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        Color color = fadeImage.color;

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}