// DragGhostHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragGhostHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventoryItem currentItem;
    private int currentQuantity;
    private ItemSlot originalSlot;


    public void BeginDrag(InventoryItem item, int quantity, ItemSlot originSlot = null)
    {
        currentItem = item;
        currentQuantity = quantity;
        originalSlot = originSlot;

        if (iconImage != null && item != null)
        {
            iconImage.sprite = item.ObjectIcon;
            iconImage.color = Color.white;
        }

        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }

        transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
    }

    public void EndDrag()
    {
        var pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        foreach (var r in results)
        {
            var hotbar = r.gameObject.GetComponentInParent<HotbarSlot>();
            if (hotbar != null && originalSlot != null)
            {
                if (currentItem is EquipmentItem eq && currentItem.itemType == InventoryItem.SlotType.Weapon)
                {
                    if (HotbarManager.Instance.EnsureWeaponInWeaponSlot(originalSlot, eq, out ItemSlot weaponSlot))
                    {
                        hotbar.AssignReference(weaponSlot);
                    }
                    else
                    {
                        ItemHoverTooltip.Instance?.ShowRaw("No empty weapon slot — can't hotbar.");
                    }
                }
                else
                {
                    if (currentItem.itemType == InventoryItem.SlotType.Food)
                    {
                        hotbar.AssignReference(originalSlot);
                    }
                }

                ResetGhost();
                return;
            }
        }

        foreach (var r in results)
        {
            var sell = r.gameObject.GetComponentInParent<SellSlot>();
            if (sell != null && originalSlot != null)
            {
                ShopManager.Instance.RegisterSellItemViaGhost(currentItem, currentQuantity);
                ResetGhost();
                return;
            }
        }

        ItemSlot targetSlot = null;
        foreach (var r in results)
        {
            targetSlot = r.gameObject.GetComponentInParent<ItemSlot>();
            if (targetSlot != null) break;
        }

        int remaining = currentQuantity;
        int movedOut = 0;

        if (targetSlot != null && targetSlot.CanAcceptItem(currentItem))
        {
            if (targetSlot.inventoryItem != null &&
                targetSlot.inventoryItem.itemId == currentItem.itemId &&
                !targetSlot.slotFilled)
            {
                int spaceLeft = targetSlot.maxHeldItems - targetSlot.heldItems;
                int add = Mathf.Min(spaceLeft, remaining);
                targetSlot.draggableIconSlot.SetQuantity(targetSlot.draggableIconSlot.quantity + add);
                targetSlot.heldItems += add;
                targetSlot.slotFilled = targetSlot.heldItems >= targetSlot.maxHeldItems;

                remaining -= add;
                movedOut += add;
            }
            else if (targetSlot.inventoryItem == null)
            {
                bool success = targetSlot.ReceiveInventoryItem(currentItem, remaining);
                if (success)
                {
                    movedOut += remaining;
                    remaining = 0;
                }
            }
        }

        if (movedOut > 0 && originalSlot != null)
        {
            originalSlot.heldItems = Mathf.Max(0, originalSlot.heldItems - movedOut);
            originalSlot.draggableIconSlot?.SetQuantity(originalSlot.heldItems);
            if (originalSlot.heldItems <= 0) originalSlot.ClearSlot();
        }

        if (remaining > 0)
        {
            if (targetSlot == null)
            {
                DropItemToWorld(remaining);

                if (originalSlot != null)
                {
                    originalSlot.heldItems = Mathf.Max(0, originalSlot.heldItems - remaining);
                    originalSlot.draggableIconSlot?.SetQuantity(originalSlot.heldItems);
                    if (originalSlot.heldItems <= 0) originalSlot.ClearSlot();
                }
            }
            else
            {
            }
        }

        ResetGhost();
    }


    private void DropItemToWorld(int amount)
    {
        if (currentItem == null || currentItem.itemPrefab == null) return;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dragDirection = (Input.mousePosition - screenCenter).normalized;
        Vector3 worldDirection = Camera.main.transform.TransformDirection(dragDirection);
        worldDirection.z = 0;
        worldDirection.Normalize();

        float baseDistance = .1f;

        for (int i = 0; i < amount; i++)
        {
            float offset = 0.1f * i;
            Vector3 dropPosition = player.position + worldDirection * (baseDistance + offset);

            RaycastHit2D hit = Physics2D.Raycast(dropPosition, Vector2.down, 5f);
            if (hit.collider != null)
            {
                dropPosition = hit.point;
            }
            ShopManager.Instance.CancelSale();
            Debug.Log("Called Late Cleanup fromm ghost");
            Instantiate(currentItem.itemPrefab, dropPosition, Quaternion.identity);
        }
    }

    private void ResetGhost()
    {
        currentItem = null;
        currentQuantity = 0;
        originalSlot = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }

        if (quantityText != null)
        {
            quantityText.text = "";
        }

        gameObject.SetActive(false);
    }
}
