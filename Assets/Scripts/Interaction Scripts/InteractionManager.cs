using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private InteractableGameObject currentTarget;
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

    private void Update()
    {
        CheckForInteractable();
    }

    public void CheckForInteractable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player with tag 'Player' not found.");
            return;
        }

        Vector2 origin = player.transform.position;
        float radius = interactionDistance;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, interactableLayer);
        

        InteractableGameObject bestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in hits)
        {
            InteractableGameObject candidate = collider.GetComponent<InteractableGameObject>();
            if (candidate == null || inventoryManager.isInventoryFull) continue;

            float distance = Vector2.Distance(origin, collider.transform.position);

            bool candidateHasInventory = candidate.inventorySO != null;
            bool bestHasInventory = bestInteractable?.inventorySO != null;

            if (
                bestInteractable == null ||
                (!bestHasInventory && candidateHasInventory) ||
                (bestHasInventory == candidateHasInventory && distance < closestDistance)
            )
            {
                bestInteractable = candidate;
                closestDistance = distance;
            }
        }

        if (bestInteractable != null)
        {
            interacting = true;
            currentTarget = bestInteractable;
            manager.InteractToolTip(interacting, currentTarget.interaction.GetPromptText());
        }
        else
        {
            interacting = false;
            currentTarget = null;
            manager.InteractToolTip(interacting, null);
        }
    }

    public void InteractWithAim()
    {
        if (currentTarget == null) return;  
        if (currentTarget != null)
        {
            currentTarget.Interact(gameObject);
            GameEventsManager.instance.interactionEvents.Interact(currentTarget.gameObject);
        }
    }
    
}
