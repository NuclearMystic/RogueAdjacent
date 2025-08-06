using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public AudioSource sfxSource;
    public AudioMixerGroup sfxMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // For global / non-positional SFX
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // For sounds that should play from a specific AudioSource (e.g., enemy, prop)
    public void PlaySFXFromSource(AudioSource source, AudioClip clip, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f)
    {
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.PlayOneShot(clip, volume);
    }
}
