using UnityEngine;

public class InteractableGameObject : MonoBehaviour
{
    
    [SerializeField] public InteractionSO interaction;
    [SerializeField] public InventoryItem inventorySO;

    public void Interact(GameObject actor)
    {
        if (interaction != null)
        {
            interaction.Execute(actor, this);
        }
    }
    
}
