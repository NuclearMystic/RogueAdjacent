using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemCreationWindow : EditorWindow
{
    private string objectName = "NewItem";
    private Sprite objectIcon;
    private bool stackable = false;
    private int stackSize = 1;
    private int itemId = 0;
    private InventoryItem.SlotType itemType = InventoryItem.SlotType.Weapon;

    private string filePathSheetOne;
    private string filePathSheetTwo;
    private string filePathSheetThree;
    private string filePathSheetFour;

    private Texture2D textureOne, textureTwo, textureThree, textureFour;

    private InteractionSO interactionScriptable;

    [MenuItem("Tools/Equipment Item Creator")]
    public static void ItemCreationWindowMethod()
    {
        GetWindow<ItemCreationWindow>("Item Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Inventory Item Fields", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        objectIcon = (Sprite)EditorGUILayout.ObjectField("Object Icon", objectIcon, typeof(Sprite), false);
        stackable = EditorGUILayout.Toggle("Stackable", stackable);
        stackSize = EditorGUILayout.IntField("Stack Size", stackSize);
        itemId = EditorGUILayout.IntField("Item ID", itemId);
        itemType = (InventoryItem.SlotType)EditorGUILayout.EnumPopup("Item Type", itemType);

        GUILayout.Space(10);
        GUILayout.Label("Equipment Item Fields", EditorStyles.boldLabel);

        Texture2D newTexOne = (Texture2D)EditorGUILayout.ObjectField("Texture One", textureOne, typeof(Texture2D), false);
        if (newTexOne != textureOne)
        {
            textureOne = newTexOne;
            filePathSheetOne = GetResourceFolderPath(textureOne);
        }
        EditorGUILayout.TextField("File Path Sheet One", filePathSheetOne);

        Texture2D newTexTwo = (Texture2D)EditorGUILayout.ObjectField("Texture Two", textureTwo, typeof(Texture2D), false);
        if (newTexTwo != textureTwo)
        {
            textureTwo = newTexTwo;
            filePathSheetTwo = GetResourceFolderPath(textureTwo);
        }
        EditorGUILayout.TextField("File Path Sheet Two", filePathSheetTwo);

        Texture2D newTexThree = (Texture2D)EditorGUILayout.ObjectField("Texture Three", textureThree, typeof(Texture2D), false);
        if (newTexThree != textureThree)
        {
            textureThree = newTexThree;
            filePathSheetThree = GetResourceFolderPath(textureThree);
        }
        EditorGUILayout.TextField("File Path Sheet Three", filePathSheetThree);

        Texture2D newTexFour = (Texture2D)EditorGUILayout.ObjectField("Texture Four", textureFour, typeof(Texture2D), false);
        if (newTexFour != textureFour)
        {
            textureFour = newTexFour;
            filePathSheetFour = GetResourceFolderPath(textureFour);
        }
        EditorGUILayout.TextField("File Path Sheet Four", filePathSheetFour);

        GUILayout.Space(10);
        interactionScriptable = (InteractionSO)EditorGUILayout.ObjectField("Interaction Scriptable", interactionScriptable, typeof(InteractionSO), false);

        GUILayout.Space(20);
        if (GUILayout.Button("Generate Item"))
        {
            CreateItem();
        }
    }

    string GetResourceFolderPath(Texture2D texture)
    {
        if (texture == null)
            return "";

        string fullPath = AssetDatabase.GetAssetPath(texture);
        const string resourcesPrefix = "Assets/Resources/";

        if (!fullPath.StartsWith(resourcesPrefix))
        {
            Debug.LogWarning($"Texture '{texture.name}' is not inside a Resources folder. Path: {fullPath}");
            return "";
        }

        string relativePath = fullPath.Substring(resourcesPrefix.Length);

        string folderPath = Path.GetDirectoryName(relativePath).Replace("\\", "/");

        if (!folderPath.EndsWith("/"))
            folderPath += "/";

        return folderPath;
    }

    void CreateItem()
    {
        string baseFolder = "Assets/GeneratedItems";
        string itemFolder = $"{baseFolder}/{objectName}";

        if (!Directory.Exists(itemFolder))
            Directory.CreateDirectory(itemFolder);

        EquipmentItem itemAsset = ScriptableObject.CreateInstance<EquipmentItem>();

        itemAsset.ObjectName = objectName;
        itemAsset.ObjectIcon = objectIcon;
        itemAsset.stackable = stackable;
        itemAsset.stackSize = stackSize;
        itemAsset.itemId = itemId;
        itemAsset.itemType = itemType;

        itemAsset.filePathSheetOne = filePathSheetOne;
        itemAsset.filePathSheetTwo = filePathSheetTwo;
        itemAsset.filePathSheetThree = filePathSheetThree;
        itemAsset.filePathSheetFour = filePathSheetFour;

        itemAsset.textureOne = textureOne;
        itemAsset.textureTwo = textureTwo;
        itemAsset.textureThree = textureThree;
        itemAsset.textureFour = textureFour;

        string assetPath = $"{itemFolder}/{objectName}.asset";
        AssetDatabase.CreateAsset(itemAsset, assetPath);

        GameObject worldItem = new GameObject(objectName + "_World");

        int interactableLayer = LayerMask.NameToLayer("Interactable");
        if (interactableLayer == -1)
        {
            Debug.LogWarning("Layer 'Interactable' does not exist! Please add it in Tags & Layers.");
        }
        else
        {
            worldItem.layer = interactableLayer;
        }

        var interactable = worldItem.AddComponent<InteractableGameObject>();
        interactable.inventorySO = itemAsset;
        interactable.interaction = interactionScriptable;

        SpriteRenderer sr = worldItem.AddComponent<SpriteRenderer>();
        sr.sprite = objectIcon;

        var collider = worldItem.AddComponent<BoxCollider2D>();
        if (sr.sprite != null)
            collider.size = sr.sprite.bounds.size;

        worldItem.transform.localScale = new Vector3(2f, 2f, 1f);

        string worldPrefabPath = $"{itemFolder}/{objectName}_World.prefab";
        PrefabUtility.SaveAsPrefabAsset(worldItem, worldPrefabPath);
        itemAsset.itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(worldPrefabPath);
        GameObject.DestroyImmediate(worldItem);

        GameObject draggableIcon = new GameObject(objectName + "_UI");
        var iconImage = draggableIcon.AddComponent<UnityEngine.UI.Image>();
        iconImage.sprite = objectIcon;
        var slot = draggableIcon.AddComponent<DraggableIconSlot>();
        slot.slotItem = itemAsset;
        slot.iconImage = iconImage;

        string uiPrefabPath = $"{itemFolder}/{objectName}_UI.prefab";
        PrefabUtility.SaveAsPrefabAsset(draggableIcon, uiPrefabPath);
        itemAsset.draggableIcon = AssetDatabase.LoadAssetAtPath<GameObject>(uiPrefabPath);
        GameObject.DestroyImmediate(draggableIcon);

        EditorUtility.SetDirty(itemAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Item '{objectName}' created in folder '{itemFolder}' with trailing slash on texture paths.");
    }
}
