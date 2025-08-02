using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class ItemCreationWindow : EditorWindow
{
    private string objectName = "NewItem";
    private Sprite objectIcon;
    private bool stackable = false;
    private int stackSize = 1;
    private int itemId = 0;
    private InventoryItem.SlotType itemType = InventoryItem.SlotType.Weapon;

    private string filePathSheetOne, filePathSheetTwo, filePathSheetThree, filePathSheetFour;
    private Texture2D textureOne, textureTwo, textureThree, textureFour;

    private TMP_FontAsset fontAsset; // ?? New font field
    private InteractionSO interactionScriptable;

    [MenuItem("Tools/Equipment Item Creator")]
    public static void ItemCreationWindowMethod()
    {
        GetWindow<ItemCreationWindow>("Item Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Inventory Item Fields", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        objectIcon = (Sprite)EditorGUILayout.ObjectField("Object Icon", objectIcon, typeof(Sprite), false);
        stackable = EditorGUILayout.Toggle("Stackable", stackable);
        stackSize = EditorGUILayout.IntField("Stack Size", Mathf.Max(stackSize, 1));
        itemId = EditorGUILayout.IntField("Item ID", itemId);
        itemType = (InventoryItem.SlotType)EditorGUILayout.EnumPopup("Item Type", itemType);

        GUILayout.Space(10);
        GUILayout.Label("Equipment Item Fields", EditorStyles.boldLabel);

        TextureField("Texture One", ref textureOne, ref filePathSheetOne);
        TextureField("Texture Two", ref textureTwo, ref filePathSheetTwo);
        TextureField("Texture Three", ref textureThree, ref filePathSheetThree);
        TextureField("Texture Four", ref textureFour, ref filePathSheetFour);

        GUILayout.Space(10);
        interactionScriptable = (InteractionSO)EditorGUILayout.ObjectField("Interaction Scriptable", interactionScriptable, typeof(InteractionSO), false);

        GUILayout.Space(10);
        GUILayout.Label("UI Font", EditorStyles.boldLabel);
        fontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Quantity Label Font", fontAsset, typeof(TMP_FontAsset), false);

        GUILayout.Space(20);
        if (GUILayout.Button("Generate Item"))
        {
            CreateItem();
        }
    }

    private void TextureField(string label, ref Texture2D tex, ref string path)
    {
        Texture2D newTex = (Texture2D)EditorGUILayout.ObjectField(label, tex, typeof(Texture2D), false);
        if (newTex != tex)
        {
            tex = newTex;
            path = GetResourceFolderPath(tex);
        }
        EditorGUILayout.TextField("Path", path);
    }

    private string GetResourceFolderPath(Texture2D texture)
    {
        if (texture == null) return "";
        string fullPath = AssetDatabase.GetAssetPath(texture);
        const string resourcesPrefix = "Assets/Resources/";

        if (!fullPath.StartsWith(resourcesPrefix))
        {
            Debug.LogWarning($"Texture '{texture.name}' is not inside a Resources folder.");
            return "";
        }

        string relativePath = fullPath.Substring(resourcesPrefix.Length);
        string folderPath = Path.GetDirectoryName(relativePath).Replace("\\", "/");

        return folderPath.EndsWith("/") ? folderPath : folderPath + "/";
    }

    private void CreateItem()
    {
        string baseFolder = "Assets/GeneratedItems";
        string itemFolder = $"{baseFolder}/{objectName}";
        if (!Directory.Exists(itemFolder)) Directory.CreateDirectory(itemFolder);

        // Create EquipmentItem asset
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

        // --- World Prefab ---
        GameObject worldItem = new GameObject(objectName + "_World");
        int interactableLayer = LayerMask.NameToLayer("Interactable");
        if (interactableLayer != -1) worldItem.layer = interactableLayer;

        var interactable = worldItem.AddComponent<InteractableGameObject>();
        interactable.inventorySO = itemAsset;
        interactable.interaction = interactionScriptable;

        SpriteRenderer sr = worldItem.AddComponent<SpriteRenderer>();
        sr.sprite = objectIcon;

        var collider = worldItem.AddComponent<BoxCollider2D>();
        if (sr.sprite != null) collider.size = sr.sprite.bounds.size;

        worldItem.transform.localScale = new Vector3(2f, 2f, 1f);

        string worldPrefabPath = $"{itemFolder}/{objectName}_World.prefab";
        PrefabUtility.SaveAsPrefabAsset(worldItem, worldPrefabPath);
        itemAsset.itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(worldPrefabPath);
        DestroyImmediate(worldItem);

        // --- UI Prefab ---
        GameObject uiIcon = new GameObject(objectName + "_UI", typeof(RectTransform));
        var iconImage = uiIcon.AddComponent<Image>();
        iconImage.sprite = objectIcon;

        var slot = uiIcon.AddComponent<DraggableIconSlot>();
        slot.slotItem = itemAsset;
        slot.iconImage = iconImage;

        // Create QuantityLabel (bottom-right)
        GameObject labelGO = new GameObject("QuantityLabel", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(uiIcon.transform, false);

        RectTransform rect = labelGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(1f, 0f);
        rect.anchoredPosition = new Vector2(-5f, 5f);
        rect.sizeDelta = new Vector2(30f, 15f);

        var quantityLabel = labelGO.GetComponent<TextMeshProUGUI>();
        quantityLabel.text = "";
        quantityLabel.fontSize = 20;
        quantityLabel.alignment = TextAlignmentOptions.BottomRight;
        quantityLabel.color = Color.white;

        if (fontAsset != null)
            quantityLabel.font = fontAsset;
        else
            Debug.LogWarning("?? Font asset was not assigned. Quantity label will use default font.");

        slot.quantityLabel = quantityLabel;

        string uiPrefabPath = $"{itemFolder}/{objectName}_UI.prefab";
        PrefabUtility.SaveAsPrefabAsset(uiIcon, uiPrefabPath);
        itemAsset.draggableIcon = AssetDatabase.LoadAssetAtPath<GameObject>(uiPrefabPath);
        DestroyImmediate(uiIcon);

        EditorUtility.SetDirty(itemAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"? Created full item: '{objectName}' with UI + world prefab + textures.");
    }
}
