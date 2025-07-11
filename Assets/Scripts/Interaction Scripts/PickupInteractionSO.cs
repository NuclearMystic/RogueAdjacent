using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Pickup Item")]
public class PickupInteractionSO : InteractionSO
{
    [SerializeField] private bool addToInventory = true;

    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        if (addToInventory)
        {

            var inventoryManager = actor.GetComponent<PlayerInventoryManager>();
            inventoryManager.PickUpItem(target.inventorySO, out addToInventory);

            if (addToInventory)
            {
                UIManager.Instance.ForceRefreshCharacterMenu();
                PlayerEquipmentManager.Instance.RefreshAllEquipmentVisuals();
                
            }
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
