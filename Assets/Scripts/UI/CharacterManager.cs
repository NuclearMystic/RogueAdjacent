using TMPro;
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
    [SerializeField] private int armorBonus;
    [SerializeField] private TMP_Text armorBonusText;

    private EquipmentItem[] currentArmorItems;
    private EquipmentItem[] currentWeaponItems;

    private PlayerEquipmentManager equipmentManager;

    public void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        questButton.onClick.RemoveAllListeners();
        skillsButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseWindow());
        questButton.onClick.AddListener(() => OpenQuestMenu());
        skillsButton.onClick.AddListener(() => OpenSkillsMenu());

        currentArmorItems = new EquipmentItem[characterArmorSlots.Length];
        currentWeaponItems = new EquipmentItem[characterWeaponSlots.Length];

        equipmentManager = PlayerEquipmentManager.Instance;
    }

    private void Update()
    {
        SyncEquippedItemsToManager();

        if (armorBonus != equipmentManager.GetArmorBonus())
        {
            armorBonus = equipmentManager.GetArmorBonus();
            Debug.Log("Armor bonus is: " + armorBonus);
            armorBonusText.text = armorBonus.ToString();
        }
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
            if (currentArmorItems[i] != item)
            {
                currentArmorItems[i] = item;
                PlayerEquipmentManager.Instance.EquipArmorItem(i, item);
            }
        }

        for (int i = 0; i < characterWeaponSlots.Length; i++)
        {
            EquipmentItem item = characterWeaponSlots[i].inventoryItem as EquipmentItem;
            if (currentWeaponItems[i] != item)
            {
                currentWeaponItems[i] = item;
                PlayerEquipmentManager.Instance.EquipWeaponItem(i, item);
            }
        }
    }
}

