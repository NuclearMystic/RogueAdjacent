using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance { get; private set; }

    public PlayerInventoryManager inventoryManager;
    private Transform firstChild;
    private PaperDoll baseLayer;
    public PaperDoll[] equipmentLayers;
    public Animator animator;

    public EquipmentItem[] equippedArmorItems = new EquipmentItem[6];
    public EquipmentItem[] equippedWeapons = new EquipmentItem[4];

    public int currentHeldWeapon;

    private GameManager gameManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;      
    }

    private void Start()
    {
        inventoryManager = GetComponent<PlayerInventoryManager>();
        firstChild = transform.GetChild(0);
        baseLayer = firstChild.GetComponent<PaperDoll>();
        animator = firstChild.GetComponent<Animator>();
        gameManager = GameManager.Instance;
        GetEdittableLayers();
        RefreshAllEquipmentVisuals();
    }

    private void GetEdittableLayers()
    {
        equipmentLayers = baseLayer.paperDollLayers;
    }

    public void RefreshAllEquipmentVisuals()
    {
        UpdateEquippedItems();
        UpdateEquippedWeapons();
    }

    private void UpdateEquippedItems()
    {
        for (int i = 0; i < equipmentLayers.Length - 1; i++)
        {
            if (equippedArmorItems[i] != null)
            {
                equipmentLayers[i].EquipNewItem(equippedArmorItems[i]);
            }
            else
            {
                equipmentLayers[i].UnequipItem();
            }
        }
    }

    private void UpdateEquippedWeapons()
    {
        if (currentHeldWeapon != 0 && equippedWeapons[currentHeldWeapon - 1] != null)
            equipmentLayers[2].EquipNewItem(equippedWeapons[currentHeldWeapon - 1]);
        else
            equipmentLayers[2].EquipNewItem(null);
    }

    public void SetCurrentHeldWeapon(int weapon)
    {
        currentHeldWeapon = weapon;
        UpdateEquippedWeapons();
    }

    public EquipmentItem GetCurrentHeldWeapon()
    {
        if (currentHeldWeapon == 0) return baseLayer.equipped;
        return equippedWeapons[currentHeldWeapon-1];
    }

    public int GetArmorBonus()
    {
        int armorBonus = 8;

        // Add base armor bonuses from equipped items  
        foreach (EquipmentItem item in equippedArmorItems)
        {
            if (item != null && item.armorBonus != 0)
            {
                armorBonus += item.armorBonus;
            }
        }

        // Add skill-based bonuses  
        float skillBonus = 0f;
        if (gameManager.playerData != null)
        {
            if (gameManager.GetPlayerClass() == PlayerClass.Fighter)
            {
                skillBonus = PlayerStats.Instance.GetSkillTotal(SkillType.HeavyArmor);
            }
            else if (gameManager.GetPlayerClass() == PlayerClass.Archer)
            {
                skillBonus = PlayerStats.Instance.GetSkillTotal(SkillType.LightArmor);
            }
            else if (gameManager.GetPlayerClass() == PlayerClass.Wizard)
            {
                skillBonus = PlayerStats.Instance.GetSkillTotal(SkillType.MageArmor);
            }
        }
        else { Debug.LogWarning("Player Data is Null"); }

        // Add 1 to armorBonus for every 10 points in the relevant skill  
        armorBonus += Mathf.FloorToInt(skillBonus / 10);

        return armorBonus;
    }

    public bool HasWeaponEquipped() => GetCurrentHeldWeapon() != null;

    public void EquipArmorItem(int index, EquipmentItem item)
    {
        if (index >= 0 && index < equippedArmorItems.Length)
        {
            equippedArmorItems[index] = item;
            UpdateEquippedItems();
        }
    }

    public void EquipWeaponItem(int index, EquipmentItem item)
    {
        if (index >= 0 && index < equippedWeapons.Length)
        {
            equippedWeapons[index] = item;
            UpdateEquippedWeapons();
        }
    }

    public void UnequipArmorItem(int index)
    {
        if (index >= 0 && index < equippedArmorItems.Length)
        {
            equippedArmorItems[index] = null;
            UpdateEquippedItems();
        }
    }

    public void UnequipWeaponItem(int index)
    {
        if (index >= 0 && index < equippedWeapons.Length)
        {
            equippedWeapons[index] = null;
            UpdateEquippedWeapons();
        }
    }
}
