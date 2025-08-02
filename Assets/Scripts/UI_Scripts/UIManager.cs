using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menu Objects")]
    public GameObject CharacterMenu;
    public GameObject InventoryMenu;
    public GameObject SkillsMenu;
    public GameObject interactTooltip;
    public GameObject QuestMenu;

    [Header("Bool Triggers")]
    public bool inventoryOpen = false;
    public bool characterOpen = false;
    public bool skillsMenuOpen = false;
    // public bool questMenuOpen = false;

    public UnityEvent updateSkillMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;

        ForceRefreshCharacterMenu();
    }

    public void Update()
    {
        ChangeCursorState();
    }

    public void ShowCharacterMenu()
    {
        ToggleCharacterMenu();
        updateSkillMenu?.Invoke();
    }

    public void ShowInventoryMenu()
    {
        ToggleInventoryMenu();
        updateSkillMenu?.Invoke();
    }

    public void ShowSkillsMenu()
    {
        ToggleSkillsMenu();
        updateSkillMenu?.Invoke();
    }

    public void ShowQuestMenu()
    {
        ToggleQuestMenu();
        updateSkillMenu?.Invoke();
    }

    private void ToggleQuestMenu()
    {
        // questMenuOpen = !questMenuOpen;
        QuestMenuManager.Instance.OpenMenu();
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

        characterOpen = !characterOpen;

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

    public void ForceRefreshCharacterMenu()
    {
        StartCoroutine(TempOpenCharacterMenu());
    }

    private IEnumerator TempOpenCharacterMenu()
    {
        bool wasOpen = characterOpen;
        bool wasIOpen = inventoryOpen;
        RectTransform menuTransform = CharacterMenu.GetComponent<RectTransform>();
        RectTransform iMTransform = InventoryMenu.GetComponent<RectTransform>();

        Vector3 originalPosition = menuTransform.anchoredPosition;
        Vector3 iMOP = iMTransform.anchoredPosition;

        if (!wasOpen)
        {
            menuTransform.anchoredPosition = new Vector2(5000, 5000);
            CharacterMenu.SetActive(true);

            yield return null;
            // yield return null;
            yield return null;
            CharacterMenu.SetActive(false);

        }
        if (!wasIOpen)
        {
            iMTransform.anchoredPosition = new Vector2(5000, 5000);
            InventoryMenu.SetActive(true);
            yield return null;
            yield return null;
            InventoryMenu.SetActive(false);
        }

        menuTransform.anchoredPosition = originalPosition;
        iMTransform.anchoredPosition = iMOP;
    }
}

