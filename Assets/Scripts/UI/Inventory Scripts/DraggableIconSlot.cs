using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class DraggableIconSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public TextMeshProUGUI quantityLabel;
    public InventoryItem slotItem;
    public int quantity = 1;
    public Transform parentAfterDrag;
    public RectTransform crosshair;

    private DragGhostHandler dragGhostHandler;
    private Canvas mainUICanvas;
    private GameObject highlightOverlayInstance;
    private bool isDragging;
    private int dragAmount;
    private bool usedGhost;
    public bool shopItem;

    private ItemSlot cachedSlot;
    private Transform cachedParent;
    private void Start()
    {
        if (crosshair == null)
        {
            GameObject crosshairGO = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshairGO != null)
                crosshair = crosshairGO.GetComponent<RectTransform>();
        }

        if (mainUICanvas == null && UIManager.Instance != null)
        {
            mainUICanvas = UIManager.Instance.GetComponentInParent<Canvas>();
        }

        dragGhostHandler = UIManager.Instance.GhostPrefab;
    }

    private void Update()
    {
        if (!isDragging)
        {
            if (slotItem == null)
            {
                iconImage.color = new Color(0, 0, 0, 0);
                UpdateQuantity(0);
                Destroy(this.gameObject);
            }
            else
            {
                iconImage.color = Color.white;
                iconImage.sprite = slotItem.ObjectIcon;
            }
            if (quantity <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetQuantity(int amount)
    {
        quantity = amount;
        UpdateQuantity(quantity);
    }

    public void UpdateQuantity(int newAmount)
    {
        quantity = newAmount;
        if (quantityLabel != null)
            quantityLabel.text = quantity > 1 ? quantity.ToString() : "";
    }
    public void SetItemAndQuantity(InventoryItem item, int qty)
    {
        slotItem = item;
        quantity = qty;
        iconImage.sprite = item.ObjectIcon;
        UpdateQuantity(qty);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (shopItem) return;
        if (slotItem != null)
        {
            ItemHoverTooltip.Instance?.Show(slotItem, this.GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shopItem) return;
        ItemHoverTooltip.Instance?.Hide();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotItem == null || dragGhostHandler == null) return;
        if (shopItem) return;

        dragAmount = quantity;
        bool usingSlider = InventoryManager.Instance.selectedItemSlot == this &&
                           InventoryManager.Instance.qtySlider != null &&
                           InventoryManager.Instance.qtySlider.gameObject.activeInHierarchy;

        if (usingSlider) dragAmount = Mathf.RoundToInt(InventoryManager.Instance.qtySlider.value);

        cachedSlot = GetComponentInParent<ItemSlot>();
        cachedParent = transform.parent;

        dragGhostHandler.BeginDrag(slotItem, dragAmount, cachedSlot);
        dragGhostHandler.gameObject.SetActive(true);
        eventData.pointerDrag = dragGhostHandler.gameObject;
        usedGhost = true;

        isDragging = false;
        iconImage.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (shopItem) return;
        if (usedGhost && dragGhostHandler != null && dragGhostHandler.isActiveAndEnabled)
        {
            dragGhostHandler.transform.position = crosshair != null ? crosshair.position : Input.mousePosition;
        }
        else
        {
            transform.position = crosshair != null ? crosshair.position : Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (shopItem) return;
        isDragging = false;
        iconImage.raycastTarget = true;

        if (usedGhost && dragGhostHandler != null && dragGhostHandler.isActiveAndEnabled)
        {
            dragGhostHandler.EndDrag();
            return;
        }

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        bool dropped = false;

        // --- SELL SLOT ---
        SellSlot sellSlot = null;
        foreach (var result in results)
        {
            sellSlot = result.gameObject.GetComponentInParent<SellSlot>();
            if (sellSlot != null) break;
        }

        if (sellSlot != null)
        {
            //Debug.Log("Dragging into sell slot");
            ShopManager.Instance.RegisterSellItem(this);

            transform.SetParent(ShopManager.Instance.sellSlotParent, false);
            transform.SetAsLastSibling();
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            dropped = true;
        }
        if (cachedParent == ShopManager.Instance.sellSlotParent && transform.parent != ShopManager.Instance.sellSlotParent)
        {
            //Debug.Log("Dragged out of sell slot — unregistering");
            ShopManager.Instance.UnregisterSellItem(this);
        }

        // --- ITEM SLOT ---
        if (!dropped)
        {
            ItemSlot targetSlot = null;
            foreach (var result in results)
            {
                targetSlot = result.gameObject.GetComponentInParent<ItemSlot>();
                if (targetSlot != null) break;
            }

            if (targetSlot != null && targetSlot.CanAcceptItem(slotItem))
            {
                bool success = targetSlot.ReceiveInventoryItem(slotItem, quantity);
                if (success)
                {
                    quantity = 0;
                    UpdateQuantity(0);
                    slotItem = null;
                    transform.SetParent(parentAfterDrag);
                    dropped = true;
                }
            }
        }

        // --- HOTBAR SLOT ---
        if (!dropped)
        {
            HotbarSlot hotbarSlot = null;
            foreach (var result in results)
            {
                hotbarSlot = result.gameObject.GetComponentInParent<HotbarSlot>();
                if (hotbarSlot != null) break;
            }

            if (hotbarSlot != null && cachedSlot != null)
            {
                if (slotItem is EquipmentItem eq && slotItem.itemType == InventoryItem.SlotType.Weapon)
                {
                    if (HotbarManager.Instance.EnsureWeaponInWeaponSlot(cachedSlot, eq, out ItemSlot weaponSlot))
                    {
                        hotbarSlot.AssignReference(weaponSlot);
                    }
                    else
                    {
                        ItemHoverTooltip.Instance?.ShowRaw("No empty weapon slot — can't hotbar.");
                        transform.SetParent(parentAfterDrag);
                        transform.localPosition = Vector3.zero;
                        return;
                    }
                }
                else
                {
                    hotbarSlot.AssignReference(cachedSlot);
                }
                transform.SetParent(parentAfterDrag);
                transform.localPosition = Vector3.zero;
                dropped = true;
                return;
            }
        }

        if (!dropped)
        {
            DropItemToWorld();
            HotbarManager.Instance?.UnassignHotbarSlotByOrigin(cachedSlot);

            quantity = 0;
            UpdateQuantity(0);
            slotItem = null;
            Destroy(gameObject);
            return;
        }

        if (transform.parent == parentAfterDrag)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shopItem) return;
        var manager = InventoryManager.Instance;
        if (manager == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemSlot originSlot = GetComponentInParent<ItemSlot>();
            if (originSlot != null)
            {
                if (slotItem is EquipmentItem eq && slotItem.itemType == InventoryItem.SlotType.Weapon)
                {
                    bool ok = HotbarManager.Instance.TryEquipToWeaponSlotAndBind(originSlot, eq);
                    if (!ok)
                    {
                        ItemHoverTooltip.Instance?.ShowRaw("No empty weapon slot\nCan't hotbar.");
                    }
                }
                else
                {
                    if (slotItem.itemType == InventoryItem.SlotType.Food)
                    {
                        var hotbar = HotbarManager.Instance;
                        if (hotbar.IsSlotAssigned(originSlot))
                            hotbar.UnassignHotbarSlotByOrigin(originSlot);
                        else
                            hotbar.AssignItemToHotbar(originSlot);
                    }
                }
            }
            return;
        }

        if (manager.selectedItemSlot == this)
        {
            RemoveHighlight();
            manager.selectedItemSlot = null;
            return;
        }

        if (manager.selectedItemSlot != null)
            manager.selectedItemSlot.RemoveHighlight();

        manager.selectedItemSlot = this;
        AddHighlight();
        manager.ShowConsumeButton();
    }

    private void DropItemToWorld()
    {
        if (slotItem == null || slotItem.itemPrefab == null) return;
        if (shopItem) return;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dragDirection = (Input.mousePosition - screenCenter).normalized;
        Vector3 worldDirection = Camera.main.transform.TransformDirection(dragDirection);
        worldDirection.z = 0;
        worldDirection.Normalize();

        float baseDistance = .1f;

        for (int i = 0; i < quantity; i++)
        {
            float offset = 0.1f * i;
            Vector3 dropPosition = player.position + worldDirection * (baseDistance + offset);

            RaycastHit2D hit = Physics2D.Raycast(dropPosition, Vector2.down, 5f);
            if (hit.collider != null)
            {
                dropPosition = hit.point;
            }
            ShopManager.Instance.CancelSale();
            Debug.Log("Called Late Cleanup");
            Instantiate(slotItem.itemPrefab, dropPosition, Quaternion.identity);
        }
    }

    public void AddHighlight()
    {
        if (highlightOverlayInstance != null) return;
        if (shopItem) return;

        GameObject prefab = InventoryManager.Instance.highlightedSlotPrefab;
        if (prefab != null)
        {
            highlightOverlayInstance = Instantiate(prefab, transform);
            // highlightOverlayInstance.GetComponent<RectTransform>().
            highlightOverlayInstance.transform.SetAsFirstSibling();
        }
    }

    public void RemoveHighlight()
    {
        if (shopItem) return;
        if (highlightOverlayInstance != null)
        {
            Destroy(highlightOverlayInstance);
            highlightOverlayInstance = null;
        }
    }
}
