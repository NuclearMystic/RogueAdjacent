using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    // The target the camera will follow
    private Transform followTarget;

    private void Start()
    {
        // Find and set the player as the follow target
        followTarget = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // If cameras position does not match the follow targets position
        if(transform.position != followTarget.position)
        {
            // Move camera to match positions with the follow target
            Vector3 camFollowVector = new Vector3(followTarget.position.x, followTarget.position.y, -10f);
            transform.position = camFollowVector;
        }
    }
}
