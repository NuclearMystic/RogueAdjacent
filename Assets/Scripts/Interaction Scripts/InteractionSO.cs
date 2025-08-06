using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Base Interaction")]
public class InteractionSO : ScriptableObject
{
    [SerializeField] public string promptText = "Press E to interact";

    public virtual void Execute(GameObject actor, InteractableGameObject target)
    {
        //Debug.Log("Interacting");
    }
    public virtual string GetPromptText() => promptText;
}
