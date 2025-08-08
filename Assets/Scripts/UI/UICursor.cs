using UnityEngine;

public class UICursor : MonoBehaviour
{
    private RectTransform crosshairRect;

    private void Awake()
    {
        crosshairRect = GetComponent<RectTransform>();

    }
    private void Update()
    {
        crosshairRect.position = Input.mousePosition;
    }
}
