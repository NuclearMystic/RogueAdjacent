using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Open Shop")]
public class ShopInteractionSO : InteractionSO
{
    [SerializeField] private ShopInfoSO shopData;

    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        ShopManager.Instance.OpenShop(shopData);
    }
}

