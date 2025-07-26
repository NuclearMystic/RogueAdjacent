using UnityEngine;


[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory Item/Equipment Item")]
public class EquipmentItem : InventoryItem
{
    public SkillType weaponSkill;
    public DiceType weaponDice;
    public int flatBonusDamage;
    public float stamDrain;
    public bool isRanged = false;

    //public Sprite[] spriteSheetOne;
    //public Sprite[] spriteSheetTwo;
    //public Sprite[] spriteSheetThree;
    //public Sprite[] spriteSheetFour;

    public string filePathSheetOne;
    public string filePathSheetTwo;
    public string filePathSheetThree;
    public string filePathSheetFour;

    public Texture2D textureOne;
    public Texture2D textureTwo;
    public Texture2D textureThree;
    public Texture2D textureFour;
}
