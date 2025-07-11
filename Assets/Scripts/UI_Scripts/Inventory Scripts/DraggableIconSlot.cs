using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DraggableIconSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  
{
    
    public Image iconImage;
    public Transform parentAfterDrag;
    public InventoryItem slotItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        iconImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        iconImage.raycastTarget = true;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DropItemToWorld();
        }
    }

    public void Awake()
    {
        if (slotItem != null)
        {
            iconImage.sprite = slotItem.ObjectIcon;
        }
    }

    public void Update()
    {
        if (slotItem == null)
        {
            iconImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            iconImage.color = Color.white;
            iconImage.sprite = slotItem.ObjectIcon;
        }
    }

    private void DropItemToWorld()
    {
        if (slotItem == null || slotItem.itemPrefab == null)
        {
            Debug.LogWarning("No item or world prefab to drop.");
            return;
        }

        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dropPosition.z = 0;

        GameObject dropped = GameObject.Instantiate(slotItem.itemPrefab, dropPosition, Quaternion.identity);

        Debug.Log($"Dropped {slotItem.ObjectName} into world at {dropPosition}");

        Destroy(gameObject);
    }
}
