using UnityEngine;

public class UIController : MonoBehaviour
{
    private Animator uiAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("button pressed");
            uiAnimator.SetTrigger("Activate");
        }
    }
}
