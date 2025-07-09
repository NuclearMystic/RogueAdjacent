using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Pickup Item")]
public class PickupInteractionSO : InteractionSO
{
    [SerializeField] private bool addToInventory = true;

    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        if (addToInventory)
        {
            actor.GetComponent<PlayerInventoryManager>().PickUpItem(target.inventorySO, out addToInventory);
        }
        if (addToInventory)
        {
            GameObject.Destroy(target.gameObject);
        }
        else
        {
            addToInventory = true;
        }
    }
}
