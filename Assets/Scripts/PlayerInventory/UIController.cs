using UnityEngine;

public class UIController : MonoBehaviour
{
    public KeyCode interactButton;
    public KeyCode inventoryButton;
    public KeyCode skillsMenuButton;
    public KeyCode characterMenuButton;

    private InteractionManager interactionManager;

    private void Start()
    {
        interactionManager = GetComponent<InteractionManager>();
    }

    private void Update()
    {
        // Interaction button
        if (Input.GetKeyDown(interactButton))
        {
            interactionManager.InteractWithAim();
        }

        // Open player inventory menu
        if (Input.GetKeyDown(inventoryButton))
        {
            UIManager.Instance.ShowInventoryMenu();
        }

        // Open player skills menu
        if (Input.GetKeyDown(skillsMenuButton))
        {
            UIManager.Instance.ShowSkillsMenu();
        }

        // Open player equipment menu
        if (Input.GetKeyDown(characterMenuButton))
        {
            UIManager.Instance.ShowCharacterMenu();
        }
    }
}
