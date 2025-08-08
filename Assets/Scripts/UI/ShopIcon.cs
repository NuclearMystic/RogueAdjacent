using UnityEngine;
using UnityEngine.EventSystems;

public class ShopIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryItem item;

    private void Start()
    {
        item = GetComponent<ShopItemSlot>().itemData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemHoverTooltip.Instance?.Show(item, this.GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemHoverTooltip.Instance?.Hide();
    }
}
