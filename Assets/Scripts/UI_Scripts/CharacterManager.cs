using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public ItemSlot[] characterItemSlots;
    public ItemSlot[] characterWeaponSlots;
    public ItemSlot[] characterArmorSlots;
    public GameObject thisMenu;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button skillsButton;

    public void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        questButton.onClick.RemoveAllListeners();
        skillsButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseWindow());
        questButton.onClick.AddListener(() => OpenQuestMenu());
        skillsButton.onClick.AddListener(() => OpenSkillsMenu());

    }
    private void Update()
    {
        SyncEquippedItemsToManager();
    }

    public void OpenQuestMenu()
    {
        UIManager.Instance.ShowQuestMenu();
    }

    public void OpenSkillsMenu()
    {
        UIManager.Instance.ShowSkillsMenu();
    }

    public void CloseWindow()
    {
        thisMenu.SetActive(false);
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

