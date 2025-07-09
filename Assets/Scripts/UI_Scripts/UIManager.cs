using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menu Objects")]
    public GameObject CharacterMenu;
    public GameObject InventoryMenu;
    public GameObject SkillsMenu;
    public GameObject interactTooltip;

    [Header("Bool Triggers")]
    public bool inventoryOpen = false;
    public bool characterOpen = false;
    public bool skillsMenuOpen = false;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;    
    }

    public void Update()
    {
        ChangeCursorState();
    }
    public void ShowCharacterMenu()
    {
        ToggleCharacterMenu();
    }
    public void ShowInventoryMenu()
    {
        ToggleInventoryMenu();
    }

    public void ShowSkillsMenu()
    {
        ToggleSkillsMenu();
    }

    private void ToggleSkillsMenu()
    {
        skillsMenuOpen = !skillsMenuOpen;
        SkillsMenu.SetActive(skillsMenuOpen);
    }

    private void ToggleInventoryMenu()
    {
        inventoryOpen = !inventoryOpen;
        InventoryMenu.SetActive(inventoryOpen);
    }

    private void ToggleCharacterMenu()
    {
        characterOpen= !characterOpen;
        CharacterMenu.SetActive(characterOpen);
    }

    public void InteractToolTip(bool tipState, string promptText)
    {
       // Debug.Log("Interacting");
        if (promptText != null)
        {
            ChangeInteractText(promptText);
            
        }
        interactTooltip.SetActive(tipState);
    }

    private void ChangeInteractText(string text)
    {
        TextMeshProUGUI interactText = interactTooltip.GetComponentInChildren<TextMeshProUGUI>();
        interactText.text = text;
    }
    private void ChangeCursorState()
    {
        if (inventoryOpen || characterOpen)
        {
           // Cursor.visible = true;
          //  Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
           // Cursor.visible = false;
           // Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
