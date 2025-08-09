using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemHoverTooltip : MonoBehaviour
{
    public static ItemHoverTooltip Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 tipOffset = new Vector2();

    private bool visible;
    private RectTransform rect;
    private Canvas canvas;

    private RectTransform owner;
    private Canvas ownerCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (tooltipText == null) Debug.LogError("Missing text reference");
        rect = tooltipText != null ? tooltipText.rectTransform : null;
        canvas  = GetComponentInParent<Canvas>();

        Hide();

    }

    void OnDisable()
    {
        visible = false;
        owner = null;
        if (rect) rect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!visible || rect == null) return;

        if (owner == null || !owner.gameObject.activeInHierarchy ||
            (ownerCanvas != null && !ownerCanvas.gameObject.activeInHierarchy))
        {
            Hide();
            return;
        }

        Vector2 targetScreenPos = (Vector2)Input.mousePosition + tipOffset;

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            var cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
            if (cam != null && RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, targetScreenPos, cam, out var world))
                rect.position = world;
            else
                rect.position = targetScreenPos;
        }
        else
        {
            rect.position = targetScreenPos;
        }
    }

    public void Show(InventoryItem item, RectTransform InOwner)
    {
       // if (item == null || owner == null) { Hide(); return; }
        owner = InOwner;
        ownerCanvas = owner.GetComponentInParent<Canvas>(); 

        tooltipText.text = ReturnFormattedText(item);
        rect.gameObject.SetActive(true);
        visible = true;
    }
    public void ShowRaw(string text)
    {
        if (tooltipText == null) return;

        tooltipText.text = text;
        rect.gameObject.SetActive(true);
        rect.gameObject.transform.position = (Vector2)Input.mousePosition + tipOffset;
    }

    public void Hide()
    {
        visible = false;
        if (rect != null) rect.gameObject.SetActive(false);
    }

    private string ReturnFormattedText(InventoryItem item)
    {
        if (item == null) return string.Empty;

        System.Text.StringBuilder sb = new System.Text.StringBuilder(128);
        sb.AppendLine($"<size=12><b>{item.ObjectName}</b></size>");

        // FOOD
        if (item.itemType == InventoryItem.SlotType.Food)
        {
            bool any = false;
            if (Mathf.Abs(item.healthEffect) > Mathf.Epsilon)
            {
                sb.AppendLine($"<size=10>+{item.healthEffect} Health</size>");
                any = true;
            }
            if (Mathf.Abs(item.staminaEffect) > Mathf.Epsilon)
            {
                sb.AppendLine($"<size=10>+{item.staminaEffect} Stamina</size>");
                any = true;
            }
            if (Mathf.Abs(item.magicEffect) > Mathf.Epsilon)
            {
                sb.AppendLine($"<size=10>+{item.magicEffect} Magic</size>");
                any = true;
            }

            if (!any)
            {
                sb.AppendLine("<size=10><i>No consumable effects</i></size>");
            }

            return sb.ToString();
        }

        // EQUIPMENT
        if (item is EquipmentItem eq)
        {
            //sb.AppendLine();
            sb.AppendLine("<size=10><b>Stats</b></size>");
            sb.AppendLine($"<size=10>Skill: {eq.weaponSkill}</size>");

            if (eq.armorBonus <= 0)
            {
                sb.AppendLine($"<size=10>Stamina Cost: {eq.stamDrain}</size>");
                sb.AppendLine($"<size=10>Flat Damage: {eq.flatBonusDamage}</size>");
            }

            // ARMOR
            else if (eq.armorBonus > 0)
            {
                sb.AppendLine($"<size=10>Armor Bonus: {eq.armorBonus}</size>");
            }

            return sb.ToString();
        }
        sb.AppendLine($"<size=10><i>{item.itemType}</i></size>");
        return sb.ToString();
    }
}
