using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menu Objects")]
    public GameObject CharacterMenu;
    public GameObject InventoryMenu;
    public GameObject SkillsMenu;
    public GameObject interactTooltip;
    public GameObject QuestMenu;
    public GameObject Crosshair;
    public GameObject ShopMenu;

    [Header("Bool Triggers")]
    public bool inventoryOpen = false;
    public bool characterOpen = false;
    public bool skillsMenuOpen = false;
    public bool refreshingMenus = false;
    // public bool questMenuOpen = false;


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
        ToggleCursor();
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

    public void ShowQuestMenu()
    {
        ToggleQuestMenu();
    }

    private void ToggleCursor()
    {
        bool anyMenuOpen = false;
        if (!refreshingMenus && (InventoryMenu.activeInHierarchy || CharacterMenu.activeInHierarchy || SkillsMenu.activeInHierarchy || QuestMenu.activeInHierarchy || ShopMenu.activeInHierarchy))
        {
            anyMenuOpen = true;
        }
        else
        {
            anyMenuOpen = false;
        }



        Crosshair.SetActive(anyMenuOpen);
        Cursor.visible = false;
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

    public void ForceRefreshCharacterMenu()
    {
        StartCoroutine(TempOpenCharacterMenu());
    }

    private IEnumerator TempOpenCharacterMenu()
    {
        refreshingMenus = true;
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
        refreshingMenus = false;
    }
}

