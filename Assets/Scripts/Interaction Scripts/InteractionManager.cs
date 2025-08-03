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
        Vector2 origin = mainCam.transform.position + Vector3.right * 0.1f;
        Vector2 direction = Vector2.up;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, interactionDistance, interactableLayer);
        Debug.DrawRay(origin, direction * interactionDistance, Color.yellow, 0.2f);

        InteractableGameObject bestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            InteractableGameObject candidate = hit.collider.GetComponent<InteractableGameObject>();

            if (candidate == null || inventoryManager.isInventoryFull)
                continue;

            bool candidateHasInventory = candidate.inventorySO != null;
            bool bestHasInventory = bestInteractable?.inventorySO != null;
           
            if (
                bestInteractable == null ||
                (!bestHasInventory && candidateHasInventory) ||
                (bestHasInventory == candidateHasInventory && hit.distance < closestDistance)
            )
            {
                bestInteractable = candidate;
                closestDistance = hit.distance;
            }
        }

        if (bestInteractable != null)
        {
            interacting = true;
            currentTarget = bestInteractable;
            manager.InteractToolTip(interacting, currentTarget.interaction.promptText);
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
        if (currentTarget != null)
        {
            currentTarget.Interact(gameObject);
            GameEventsManager.instance.interactionEvents.Interact(currentTarget.gameObject);
        }
    }
}
