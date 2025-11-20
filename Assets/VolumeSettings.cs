using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingSimple : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider volumeSlider;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    private const string VOLUME_KEY = "MusicVolume";

    void Start()
    {
        // Load saved volume or use default (0.75)
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.75f);

        // Apply it
        musicSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Connect slider event
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
    }
}
