using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public ItemSlot[] characterItemSlots;
    public ItemSlot[] characterWeaponSlots;
    public ItemSlot[] characterArmorSlots;

    private void Update()
    {
        SyncEquippedItemsToManager();
    }

    private void SyncEquippedItemsToManager()
    {
        for (int i = 0; i < characterArmorSlots.Length; i++)
        {
            EquipmentItem item = characterArmorSlots[i].inventoryItem as EquipmentItem;
            PlayerEquipmentManager.Instance.EquipArmorItem(i, item);
        }

        for (int i = 0; i < characterWeaponSlots.Length; i++)
        {
            EquipmentItem item = characterWeaponSlots[i].inventoryItem as EquipmentItem;
            PlayerEquipmentManager.Instance.EquipWeaponItem(i, item);
        }
    }
}