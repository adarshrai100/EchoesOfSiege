using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Music")]

    [SerializeField] private AudioClip _gameplayMusic;

    [SerializeField] private float _musicVolume = 0.5f;

    private AudioSource _musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _archerShootClip;
    [SerializeField] private AudioClip _ballistaShootClip;
    [SerializeField] private AudioClip _enemyHitClip;
    [SerializeField] private AudioClip _enemyDeathClip;
    [SerializeField] private AudioClip _castleHitClip;
    [SerializeField] private AudioClip _waveStartClip;
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

        _musicSource = gameObject.AddComponent<AudioSource>();

        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.volume = _musicVolume * _masterVolume;
    }

    public void PlayArcherShoot() => Play(_archerShootClip, _shootVolume);

    public void PlayBallistaShoot() => Play(_ballistaShootClip, _shootVolume);

    public void PlayEnemyHit() => Play(_enemyHitClip, _hitVolume);

    public void PlayEnemyDeath() => Play(_enemyDeathClip, _hitVolume);

    public void PlayCastleHit() => Play(_castleHitClip, _hitVolume);

    public void PlayWaveStart() => Play(_waveStartClip, _upgradeVolume);

    public void PlayUpgrade() => Play(_upgradeClip, _upgradeVolume);

    public void PlaySell() => Play(_sellClip, _sellVolume);

    public void PlayGameOver() => Play(_gameOverClip, _gameOverVolume);

    public void PlayShoot() => PlayArcherShoot();
    public void PlayHit() => PlayEnemyHit();

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clip, volume * _masterVolume);
            _audioSource.pitch = 1f;
        }
    }

    public void PlayGameplayMusic()
    {
        if (_gameplayMusic == null)
            return;

        if (_musicSource.isPlaying)
            return;

        _musicSource.clip = _gameplayMusic;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}