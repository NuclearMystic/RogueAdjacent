using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] 
    private InteractableGameObject currentTarget;
    public UIManager manager;   
    public PlayerInventoryManager inventoryManager;
    public LayerMask interactableLayer;
    private Camera mainCam;
    
    [Header("Raycast Settings")]

    public float interactionDistance = 5f;   

    public Camera playerCamera;

    [Header("Triggers")]
    public bool interacting = false;
    private void Start()
    {
        manager = FindFirstObjectByType<UIManager>();    
        inventoryManager = GetComponent<PlayerInventoryManager>();
        mainCam = Camera.main;
    }

    void Update()
    {
        CheckForInteractable();
        //InteractWithAim();       
    }

    public void CheckForInteractable()
    {

        Vector2 origin = mainCam.transform.position + Vector3.right *0.1f;

        Vector2 direction = Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactionDistance, interactableLayer);

        Debug.DrawRay(origin, direction * interactionDistance, Color.yellow, 0.2f);

        if (hit.collider != null)
        {          
            InteractableGameObject interactable = hit.collider.GetComponent<InteractableGameObject>();
            if (interactable != null && !inventoryManager.isInventoryFull)
            {            
                interacting = true;
                currentTarget = interactable;
                manager.InteractToolTip(interacting, currentTarget.interaction.promptText);                             
                return;
            }
        }
        interacting = false;
        currentTarget = null;
        manager.InteractToolTip(interacting, null);
    }

    public void InteractWithAim()
    {
        if (currentTarget != null)
        {
            currentTarget.Interact(gameObject);
        }
    }
}
