using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider ambienceSlider;
    public AudioSource ambienceSource;

    void Start()
    {
        if (PlayerPrefs.HasKey("AmbienceVolume"))
        {
            float vol = PlayerPrefs.GetFloat("AmbienceVolume");
            ambienceSlider.value = vol;
            ambienceSource.volume = vol;
        }

        ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
    }

    public void SetAmbienceVolume(float volume)
    {
        ambienceSource.volume = volume;

        PlayerPrefs.SetFloat("AmbienceVolume", volume);
    }
}
