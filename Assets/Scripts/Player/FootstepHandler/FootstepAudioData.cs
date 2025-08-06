using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Footstep Data")]
public class FootstepAudioData : ScriptableObject
{
    public SurfaceType surfaceType;
    public AudioClip[] footstepClips;
}