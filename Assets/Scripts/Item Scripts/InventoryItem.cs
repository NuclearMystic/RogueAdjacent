using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string ObjectName;
    public Sprite ObjectIcon;
    public bool stackable;
    public int stackSize = 1;
    public int itemId;
    
        

    public enum SlotType
    {
        Food,
        Weapon,
        Hair,
        Hat,
        FaceAcces,
        Cape,
        Outfit       
    }

    public SlotType itemType;
    public GameObject itemPrefab;
    public GameObject draggableIcon;
}
