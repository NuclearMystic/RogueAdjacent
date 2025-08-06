using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    [Header("Base Info")]
    public string ObjectName;
    public Sprite ObjectIcon;
    public bool stackable;
    public int stackSize = 1;
    public int itemId;
    public int baseCost;

    [Header("Consumables Info")]
    public float healthEffect;
    public float staminaEffect;
    public float magicEffect;

    public AudioClip itemPickedUpSFX;
    public AudioClip itemUsedSFX;

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
