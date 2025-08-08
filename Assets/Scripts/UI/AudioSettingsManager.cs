using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    private const string MasterKey = "vol_master";
    private const string MusicKey = "vol_music";
    private const string SfxKey = "vol_sfx";
    private const string AmbKey = "vol_amb";

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambSlider;

    private void Awake()
    {
        // 1) Load and APPLY to mixer immediately (before audio plays)
        ApplyToMixer(PlayerPrefs.GetFloat(MasterKey, 0.8f), "masterVolume");
        ApplyToMixer(PlayerPrefs.GetFloat(MusicKey, 0.4f), "musicVolume");
        ApplyToMixer(PlayerPrefs.GetFloat(SfxKey, 0.8f), "sfxVolume");
        ApplyToMixer(PlayerPrefs.GetFloat(AmbKey, 0.8f), "ambienceVolume");
    }

    private void Start()
    {
        // 2) Sync sliders without triggering callbacks
        masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(MasterKey, 0.8f));
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(MusicKey, 0.8f));
        sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(SfxKey, 0.8f));
        ambSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(AmbKey, 0.8f));

        // 3) Now wire listeners
        masterSlider.onValueChanged.AddListener(v => OnChange(MasterKey, v, "masterVolume"));
        musicSlider.onValueChanged.AddListener(v => OnChange(MusicKey, v, "musicVolume"));
        sfxSlider.onValueChanged.AddListener(v => OnChange(SfxKey, v, "sfxVolume"));
        ambSlider.onValueChanged.AddListener(v => OnChange(AmbKey, v, "ambienceVolume"));
    }

    private void OnChange(string key, float linear, string param)
    {
        PlayerPrefs.SetFloat(key, linear);
        PlayerPrefs.Save();
        ApplyToMixer(linear, param);
    }

    private void ApplyToMixer(float linear, string param)
    {
        linear = Mathf.Clamp01(linear);
        float dB = linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
        mixer.SetFloat(param, dB);
    }
}
