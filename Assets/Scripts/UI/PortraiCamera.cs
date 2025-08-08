using UnityEngine;

public class PortraitCamera : MonoBehaviour
{
    
    public Transform target;
    public Vector2 offset = new Vector2(0f, 0.5f); 
    public float fixedZ = -10f;

    public float smoothSpeed = 5f;

    private void Start()
    {
        if (target == null)
        {
            TopDownCharacterController controller = FindFirstObjectByType<TopDownCharacterController>();
            if (controller != null)
                target = controller.transform;
            else
                Debug.LogWarning("No TopDownCharacterController found.");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            fixedZ
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
