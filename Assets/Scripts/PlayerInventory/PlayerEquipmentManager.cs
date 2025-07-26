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

        GetEdittableLayers();
        RefreshAllEquipmentVisuals();

    }

    private void Update()
    {
        

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
        {
            equipmentLayers[4].EquipNewItem(equippedWeapons[currentHeldWeapon - 1]);
        }
        else
        {
            equipmentLayers[4].EquipNewItem(null);

        }

    }

    public void SetCurrentHeldWeapon(int weapon)
    {

        currentHeldWeapon = (weapon != currentHeldWeapon) ? weapon : 0;
        UpdateEquippedWeapons();
    }

    public EquipmentItem GetCurrentHeldWeapon()
    {
        return equippedWeapons[currentHeldWeapon - 1];
    }

    public bool HasWeaponEquipped()
    {
        if(GetCurrentHeldWeapon() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
