using System.Linq;

using UnityEngine;


public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance { get; private set; }

    public PlayerInventoryManager inventoryManager;
    
    private Transform firstChild;
    private PaperDoll baseLayer;
    public PaperDoll[] equipmentLayers;
    public ItemSlot[] equipmentSlots;
    public ItemSlot[] weaponSlots;
    public Animator animator;
    
    public EquipmentItem[] equipmentItems;
    public EquipmentItem[] equippedWeapons;
    
    
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
        
        Transform transform = this.transform;
        firstChild = transform.GetChild(0);
        baseLayer = firstChild.GetComponent<PaperDoll>();
        animator = firstChild.GetComponent<Animator>();

        GetEdittableLayers();
        GetEquippedItemSlots();
    }
    
    private void Update()
    {
        UpdateEquippedItems();
        UpdateEquippedWeapons();
    }

    private void GetEdittableLayers()
    {
        equipmentLayers = baseLayer.paperDollLayers;
        //int foundLayer = 0;

        //for (int i = 0; i < baseLayer.paperDollLayers.Length; i++) {

        //    if (baseLayer.paperDollLayers[i].edittable)
        //    {                              
        //       equipmentLayers[foundLayer] = baseLayer.paperDollLayers[i];
        //        foundLayer++;
        //    }
        //}

       
    }
    private void GetEquippedItemSlots()
    {
        equipmentSlots = inventoryManager.characterManager.characterArmorSlots.ToArray();
        weaponSlots = inventoryManager.characterManager.characterWeaponSlots.ToArray();
    }

    private void UpdateEquippedItems()
    {
        for (int i = 0; i < 3; i++)
        {
            if (equipmentSlots[i].inventoryItem != null && equipmentSlots[i].inventoryItem is EquipmentItem equipItem)
            {
                equipmentLayers[i].EquipNewItem((EquipmentItem)equipmentSlots[i].inventoryItem);
            }
        }

    }
    
    private void UpdateEquippedWeapons()
    {
        if (currentHeldWeapon != 0 && weaponSlots[currentHeldWeapon - 1] != null)
        {
            equipmentLayers[3].EquipNewItem((EquipmentItem)weaponSlots[currentHeldWeapon - 1].inventoryItem);
        }
        else
        {
            equipmentLayers[3].EquipNewItem(null);
        }
    }

    public void SetCurrentHeldWeapon(int weapon)
    {
        if (weapon != currentHeldWeapon)
        {
            currentHeldWeapon = weapon;
        }
        else
        {
            currentHeldWeapon = 0;
        }
    }
    

    

    
}
