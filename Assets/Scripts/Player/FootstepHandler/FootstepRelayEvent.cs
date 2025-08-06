using UnityEngine;

public class FootstepRelayEvent : MonoBehaviour
{
    public FootstepHandler footstepHandler;

    public void PlayFootstepSFX()
    {
        footstepHandler.PlayFootstep();
    }
}
