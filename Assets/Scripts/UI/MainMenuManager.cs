using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance?.PlayMenuMusic();
    }

    public void PlayGame()
    {
        SceneTransition.Instance.LoadScene("Arena_Main");
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}