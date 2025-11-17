using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Display Settings")]
    public Toggle fullscreenToggle;

    private const string FULLSCREEN_KEY = "Fullscreen";

    void Start()
    {
        // ... (Bagian loading status dan listener AddListener sudah benar)
        int savedFullscreenState = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0);
        bool isFullscreen = savedFullscreenState == 1;

        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;

        // Listener ini akan memanggil fungsi di bawah secara otomatis
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    // ✨ KOREKSI: Tambahkan kata kunci 'public' di sini
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}