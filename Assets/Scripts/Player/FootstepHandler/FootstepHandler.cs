using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    public List<FootstepAudioData> footstepAudioSets;
    public SFXManager sfxManager;

    private Dictionary<SurfaceType, AudioClip[]> footstepMap;
    private SurfaceType currentSurface = SurfaceType.Default;

    private void Awake()
    {
        footstepMap = new Dictionary<SurfaceType, AudioClip[]>();
        foreach (var data in footstepAudioSets)
        {
            footstepMap[data.surfaceType] = data.footstepClips;
        }
    }

    public void PlayFootstep()
    {
        if (!footstepMap.TryGetValue(currentSurface, out AudioClip[] clips))
        {
            clips = footstepMap[SurfaceType.Default];
        }

        if (clips != null && clips.Length > 0)
        {
            AudioClip chosen = clips[Random.Range(0, clips.Length)];
            sfxManager.PlaySFX(chosen);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SurfaceIdentifier surface = other.GetComponent<SurfaceIdentifier>();
        if (surface != null)
        {
            currentSurface = surface.surfaceType;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SurfaceIdentifier surface = other.GetComponent<SurfaceIdentifier>();
        if (surface != null && surface.surfaceType == currentSurface)
        {
            currentSurface = SurfaceType.Default;
        }
    }
}
