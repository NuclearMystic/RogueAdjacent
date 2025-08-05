using UnityEngine;

public class UIController : MonoBehaviour
{
    public KeyCode interactButton;
    public KeyCode inventoryButton;
    public KeyCode skillsMenuButton;
    public KeyCode characterMenuButton;
    public KeyCode openAllMenus;
    public KeyCode systemMenuButton;

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

        // Open all menus at once
        if (Input.GetKeyDown(openAllMenus))
        {
            UIManager.Instance.ShowInventoryMenu();
            UIManager.Instance.ShowSkillsMenu();
            UIManager.Instance.ShowCharacterMenu();
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

        // Open system menu
        if (Input.GetKeyDown(systemMenuButton))
        {
            UIManager.Instance.ShowSystemMenu();
        }
    }
}
