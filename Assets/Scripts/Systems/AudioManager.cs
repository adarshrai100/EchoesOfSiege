using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _upgradeClip;
    [SerializeField] private AudioClip _sellClip;
    [SerializeField] private AudioClip _gameOverClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayShoot() => Play(_shootClip);
    public void PlayHit() => Play(_hitClip);
    public void PlayUpgrade() => Play(_upgradeClip);
    public void PlaySell() => Play(_sellClip);
    public void PlayGameOver() => Play(_gameOverClip);

    private void Play(AudioClip clip)
    {
        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}