using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastleController : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashDuration = 0.15f;

    [SerializeField] private float _shakeDuration = 0.12f;
    [SerializeField] private float _shakeStrength = 0.08f;

    [SerializeField] private ParticleSystem _damageParticles;

    private Vector3 _originalPosition;

    private Color[] _originalColors;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
        if (_renderers == null || _renderers.Length == 0)
            _renderers = GetComponentsInChildren<Renderer>();

        _originalColors = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalColors[i] = _renderers[i].material.color;
        }
    }

    public void PlayDamageFeedback()
    {
        StopAllCoroutines();
        AudioManager.Instance?.PlayCastleHit();
        _damageParticles?.Play();

        StartCoroutine(DamageRoutine());
    }


    private IEnumerator DamageRoutine()
    {
        // Flash red
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _flashColor;
        }

        float timer = 0f;

        while (timer < _shakeDuration)
        {
            timer += Time.deltaTime;

            transform.localPosition =
                _originalPosition + Random.insideUnitSphere * _shakeStrength;

            yield return null;
        }

        transform.localPosition = _originalPosition;

        // Restore colors
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _originalColors[i];
        }
    }

}