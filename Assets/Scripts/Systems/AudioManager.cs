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

    [SerializeField] private float _masterVolume = 1f;

    [SerializeField] private float _shootVolume = 0.5f;
    [SerializeField] private float _hitVolume = 0.4f;
    [SerializeField] private float _upgradeVolume = 0.7f;
    [SerializeField] private float _sellVolume = 0.7f;
    [SerializeField] private float _gameOverVolume = 0.8f;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayShoot() => Play(_shootClip, _shootVolume);
    public void PlayHit() => Play(_hitClip, _hitVolume);
    public void PlayUpgrade() => Play(_upgradeClip, _upgradeVolume);
    public void PlaySell() => Play(_sellClip, _sellVolume);
    public void PlayGameOver() => Play(_gameOverClip, _gameOverVolume);

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clip, volume * _masterVolume);
            _audioSource.pitch = 1f;
        }
    }
}