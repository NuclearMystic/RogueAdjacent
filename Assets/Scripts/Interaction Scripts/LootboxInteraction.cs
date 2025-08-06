using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Lootbox Interaction")]
public class LootboxInteraction : InteractionSO
{
    
    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        LootBoxManager box = target.GetComponent<LootBoxManager>();
        if (box != null && !box.hasBeenOpened)
        {
            box.OnOpen();
        }
        
    }

    public override string GetPromptText()
    {
        return "'E' to Open Loot Box";
    }
}
