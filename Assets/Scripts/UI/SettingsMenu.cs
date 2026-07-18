using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    public void Open()
    {
        settingsPanel.SetActive(true);
    }

    public void Close()
    {
        settingsPanel.SetActive(false);
    }

    private void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    private void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
}