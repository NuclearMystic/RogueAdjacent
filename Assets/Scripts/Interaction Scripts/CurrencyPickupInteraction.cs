using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Currency Pickup Item")]
public class CurrencyPickupInteraction : InteractionSO
{
    [SerializeField] private bool addToInventory = true;
    [SerializeField] private float currency = 5;
    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        if (addToInventory)
        {
            GameEventsManager.instance.currencyEvents.CurrencyGained(currency);
        }


        if (addToInventory)
        {
            GameObject.Destroy(target.gameObject);
        }
        else
        {
            addToInventory = true;
        }
    }
}
