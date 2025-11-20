using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionPanel;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;         
    [SerializeField] private float fadeDuration = 1f; 

    [Header("Audio")]
    [SerializeField] private AudioSource menuMusic;   

    private void Update()
    {
        // ESC closes options and returns to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
            {
                CloseOptions();
            }
        }
    }

    public void PlayGame()
    {
        StartCoroutine(FadeOutAndPlay());
    }

    private IEnumerator FadeOutAndPlay()
    {
        float t = 0f;

        Color color = fadeImage.color;
        float startVolume = menuMusic != null ? menuMusic.volume : 1f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float progress = t / fadeDuration;

            color.a = progress;
            fadeImage.color = color;

            if (menuMusic != null)
                menuMusic.volume = Mathf.Lerp(startVolume, 0f, progress);

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        if (menuMusic != null)
            menuMusic.volume = 0f;

        SceneManager.LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (optionPanel != null)
            optionPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionPanel != null)
            optionPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
