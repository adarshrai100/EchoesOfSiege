using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Music")]

    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameplayMusic;
    [SerializeField] private AudioClip _victoryMusic;
    [SerializeField] private AudioClip _gameOverMusic;

    [SerializeField] private float _musicVolume = 1f;
    [SerializeField] private float _menuMusicVolume = 1.5f;
    [SerializeField] private float _gameplayMusicVolume = 0.5f;
    [SerializeField] private float _victoryMusicVolume = 0.7f;

    private AudioSource _musicSource;
    private float _currentMusicTargetVolume = 1f;

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
    [SerializeField] private AudioClip _uiHoverClip;
    [SerializeField] private AudioClip _uiClickClip;



    [SerializeField] private float _masterVolume = 1f;

    [SerializeField] private float _shootVolume = 0.5f;
    [SerializeField] private float _hitVolume = 0.4f;
    [SerializeField] private float _upgradeVolume = 0.7f;
    [SerializeField] private float _sellVolume = 0.7f;
    [SerializeField] private float _gameOverVolume = 0.8f;
    [SerializeField] private float _gameOverMusicVolume = 0.7f;

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

    private Coroutine _musicFadeCoroutine;

    private IEnumerator FadeMusic(float targetVolume, float duration)
    {
        float startVolume = _musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        _musicSource.volume = targetVolume;
    }

    private void StartMusicFade(float targetVolume, float duration)
    {
        if (_musicFadeCoroutine != null)
            StopCoroutine(_musicFadeCoroutine);

        _musicFadeCoroutine = StartCoroutine(FadeMusic(targetVolume, duration));
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

    public void PlayMenuMusic()
    {
        PlayMusic(_menuMusic, _menuMusicVolume);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(_gameplayMusic, _gameplayMusicVolume);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(_victoryMusic, _victoryMusicVolume);
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(clip, volume * _masterVolume);
            _audioSource.pitch = 1f;
        }
    }

    private void PlayMusic(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        if (_musicSource.clip == clip && _musicSource.isPlaying)
            return;

        _currentMusicTargetVolume = volume;

        _musicSource.Stop();

        _musicSource.clip = clip;
        _musicSource.volume = 0f;
        _musicSource.Play();

        StartMusicFade(_currentMusicTargetVolume * _musicVolume * _masterVolume, 2f);
    }


    public void PlayUIHover()
    {
        Play(_uiHoverClip, 0.9f);
    }

    public void PlayUIClick()
    {
        Play(_uiClickClip, 0.9f);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;

        _musicSource.volume =
            _currentMusicTargetVolume *
            _musicVolume *
            _masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _masterVolume = volume;

        _musicSource.volume =
            _currentMusicTargetVolume *
            _musicVolume *
            _masterVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public float GetSFXVolume()
    {
        return _masterVolume;
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(_gameOverMusic, _gameOverMusicVolume);
    }
}