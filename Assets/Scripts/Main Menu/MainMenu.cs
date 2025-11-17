using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;  // drag panel utama
    public GameObject optionPanel;    // drag panel opsi

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (optionPanel != null)
            optionPanel.SetActive(true);

        Debug.Log("Option menu terbuka!");
    }

    public void CloseOptions()
    {
        if (optionPanel != null)
            optionPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Debug.Log("Option menu ditutup!");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar game...");
        Application.Quit();
    }
}
