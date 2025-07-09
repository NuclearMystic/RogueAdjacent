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
            // Convert the replacementTexture to sprites
            LoadSpritesFromTexture();
        }
        else
        {
            // If replacementTexture is null, disable the script component
            //enabled = false;
            //Debug.LogWarning("Replacement texture is not assigned. PaperDoll component disabled.");
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

        if (!isChildSprite)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].textureRect.Equals(currentBaseSprite.textureRect))
                {
                    currentBaseSpriteIndex = i;
                    break;
                }
            }


        }
        else
        {
            // Sync to the parent/base sprite
            PaperDoll parent = baseRenderer.GetComponent<PaperDoll>();
            if (parent != null && parent.sprites != null && parent.sprites.Length > 0)
            {
               // Debug.Log("changing sprite index");
                currentBaseSpriteIndex = parent.currentBaseSpriteIndex;

                // Only assign if within bounds
                if (sprites != null && currentBaseSpriteIndex < sprites.Length)
                {
                    GetComponent<SpriteRenderer>().sprite = sprites[currentBaseSpriteIndex];
                }
            }
        }

        LayerSorting();
    }

    private void LayerSorting()
    {
        if (isChildSprite && layerType == LayerType.Weapon)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            //Animator animator = PlayerEquipmentManager.Instance.animator;

            // Example: Get movement input
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Change sorting order based on vertical movement
            if (verticalInput > 0) // Moving upwards
            {
                spriteRenderer.sortingOrder = 1; // Or a higher value
            }
            else if (verticalInput < 0) // Moving downwards
            {
                spriteRenderer.sortingOrder = -1; // Or a lower value
            }
            else // Not moving vertically
            {
               // spriteRenderer.sortingOrder = 0; // Default sorting order
            }
        }

    }


    private void LoadSpritesFromTexture()
    {
        // Get the individual sprites from the replacement texture
        // its just loading sprites into the array
        sprites = Resources.LoadAll<Sprite>(path + replacementTexture.name);
        
    }

    public void SwapToBaseSheet()
    {
        // Swap back to the idle sprite sheet
        replacementTexture = equipped.textureOne;
        path = equipped.filePathSheetOne;
        LoadSpritesFromTexture();

        if (!isChildSprite)
        {
            foreach (PaperDoll paperDoll in paperDollLayers)
            {
                paperDoll.SwapToBaseSheet();
            }
        }
    }

    public void UpdateTest()
    {
        SwapToBaseSheet();
        swapTest = false;
    }

    public void SwapToAttackSheet()
    {
        // Swap to the attack sprite sheet
        replacementTexture = equipped.textureTwo;
        path = equipped.filePathSheetTwo;
        LoadSpritesFromTexture();

        if (!isChildSprite)
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
}
