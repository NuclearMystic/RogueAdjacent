using UnityEngine;

public class PaperDoll : MonoBehaviour
{
    [Header("Replacement Textures")]
    [Tooltip("Reference to the base sprite for the character.")]
    [SerializeField] public SpriteRenderer baseRenderer;
    [Tooltip("The texture to replace for this layer.")]
    [SerializeField] private Texture2D replacementTexture;
    [Tooltip("File path for the replacement texture;")]
    [SerializeField] private string path;

    [Header("Equipment Slots")]
    [Tooltip("If this is the base renderer add the child object Paper doll scripts")]
    public PaperDoll[] paperDollLayers;
    [Tooltip("If this is a child object renderer just add the equipment object currently worn on this layer")]
    public EquipmentItem equipped;

    [HideInInspector]
    public Sprite[] sprites; // Array of sprites from the sprite sheet
    private Sprite currentBaseSprite;
    public int currentBaseSpriteIndex;

    public bool isChildSprite;
    public bool equippedWeaponIsRanged;

    public enum LayerType
    {
        Parent,
        Weapon,
        Hair,
        Hat,
        FaceAcces,
        Cape,
        Outfit
    }

    public LayerType layerType;
    public bool edittable;
    public bool swapTest = false;

    private void Start()
    {
        if (replacementTexture != null)
        {
            LoadSpritesFromTexture();
        }
        else if (equipped != null)
        {
            UpdateTest(); // Fallback to load from equipped item
        }
    }

    private void Update()
    {
        currentBaseSprite = baseRenderer.sprite;

        if (equipped != null && replacementTexture == null)
        {
            UpdateTest();
        }

        if (baseRenderer == null && equipped != null)
        {
            UpdateTest();
        }

        // FRAME SYNC FIX:
        if (!isChildSprite)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                // Compare sprite texture rectangles to find the index
                if (sprites[i].textureRect.Equals(currentBaseSprite.textureRect))
                {
                    currentBaseSpriteIndex = i;
                    break;
                }
            }
        }
        else
        {
            // Child object: just follow parent's resolved index
            PaperDoll parent = baseRenderer.GetComponent<PaperDoll>();
            if (parent != null && parent.sprites != null && parent.sprites.Length > 0)
            {
                currentBaseSpriteIndex = parent.currentBaseSpriteIndex;

                if (sprites != null && currentBaseSpriteIndex < sprites.Length)
                {
                    GetComponent<SpriteRenderer>().sprite = sprites[currentBaseSpriteIndex];
                }
            }
        }

        LayerSorting();
        CheckForUnequip();
    }

    private void LayerSorting()
    {
        if (isChildSprite && layerType == LayerType.Weapon)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            //Animator animator = PlayerEquipmentManager.Instance.animator;

            // Example: Get movement input
            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (equipped != null && equipped.isRanged)
            {
                equippedWeaponIsRanged = true;
            }
            else
            {
                equippedWeaponIsRanged = false;
            }

            // Change sorting order based on vertical movement
            if (verticalInput == 1) // Moving upwards
            {
                spriteRenderer.sortingOrder = 2; // Or a higher value
            }
            else if (verticalInput == -1) // Moving downwards
            {
                spriteRenderer.sortingOrder = -1; // Or a lower value
            }
            else if(verticalInput == 0 && horizontalInput != 0) // moving left and right
            {
                spriteRenderer.sortingOrder = -1; // Or a lower value
            }
            else // Not moving vertically
            {
                // spriteRenderer.sortingOrder = 0; // Default sorting order
            }
        }

    }

    private void CheckForUnequip()
    {
        if (isChildSprite && equipped == null)
        {
            UnequipItem();
        }
    }

    private void LoadSpritesFromTexture()
    {
        sprites = Resources.LoadAll<Sprite>(path + replacementTexture.name);
    }

    public void SwapToBaseSheet()
    {
        if (equipped != null)
        {
            // Swap back to the idle sprite sheet
            replacementTexture = equipped.textureOne;
            path = equipped.filePathSheetOne;
            LoadSpritesFromTexture();
        }

        if (!isChildSprite)
        {
            foreach (PaperDoll paperDoll in paperDollLayers)
            {
                paperDoll.SwapToBaseSheet();
            }
        }
    }

    public void SwapToAttackSheet()
    {
        if (equipped != null)
        {
            Debug.Log($"[PaperDoll] Swapping {gameObject.name} to {equipped.textureTwo.name}");

            if (equippedWeaponIsRanged && layerType != LayerType.Weapon)
            {
                replacementTexture = equipped.textureThree;
                path = equipped.filePathSheetThree;
            }
            else
            {
                replacementTexture = equipped.textureTwo;
                path = equipped.filePathSheetTwo;
            }
            LoadSpritesFromTexture();
        }
        else
        {
            Debug.LogWarning($"[PaperDoll] No equipped item on {gameObject.name}, can't swap to attack sheet.");
        }

        if (!isChildSprite && paperDollLayers != null)
        {
            foreach (PaperDoll paperDoll in paperDollLayers)
            {
                paperDoll.SwapToAttackSheet();
            }
        }
    }

    public void SwapAllSheetOne()
    {
        if (!isChildSprite)
        {
            foreach (PaperDoll paperDoll in paperDollLayers)
            {
                paperDoll.SwapToBaseSheet();
            }
        }
    }

    public void SwapAllSheetTwo()
    {
        if(!isChildSprite)
        {
            foreach(PaperDoll paperDoll in paperDollLayers)
            {
                paperDoll.SwapToAttackSheet();
            }
        }
    }

    public void EquipNewItem(EquipmentItem item)
    {
        equipped = item;

        if (equipped != null)
        {
            replacementTexture = equipped.textureOne;
            path = equipped.filePathSheetOne;
            LoadSpritesFromTexture();
        }

        if (!isChildSprite && paperDollLayers != null)
        {
            foreach (PaperDoll child in paperDollLayers)
            {
                child.EquipNewItem(item);
            }
        }
    }

    public void UnequipItem()
    {
        equipped = null;
        path = null;
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void UpdateTest()
    {
        SwapToBaseSheet();
    }
}
