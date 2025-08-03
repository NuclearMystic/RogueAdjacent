using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Runtime State (Read-Only)")]
    public bool InventoryOpen => InventoryMenu.activeInHierarchy;
    public bool CharacterOpen => CharacterMenu.activeInHierarchy;
    public bool SkillsMenuOpen => SkillsMenu.activeInHierarchy;
    public bool QuestMenuOpen => QuestMenu.activeInHierarchy;
    public bool ShopMenuOpen => ShopMenu.activeInHierarchy;

    private bool refreshingMenus = false;

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

    private void Update()
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
        if (!refreshingMenus && (InventoryOpen || CharacterOpen || SkillsMenuOpen || QuestMenuOpen || ShopMenuOpen))
        {
            anyMenuOpen = true;
        }

        Crosshair.SetActive(anyMenuOpen);
        Cursor.visible = false;
    }

    private void ToggleQuestMenu()
    {
        QuestMenuManager.Instance.OpenMenu(); // You handle this separately, assumed to toggle it internally
    }

    private void ToggleSkillsMenu()
    {
        SkillsMenu.SetActive(!SkillsMenu.activeInHierarchy);
    }

    private void ToggleInventoryMenu()
    {
        InventoryMenu.SetActive(!InventoryMenu.activeInHierarchy);
    }

    private void ToggleCharacterMenu()
    {
        CharacterMenu.SetActive(!CharacterMenu.activeInHierarchy);
    }

    public void InteractToolTip(bool tipState, string promptText)
    {
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

        RectTransform menuTransform = CharacterMenu.GetComponent<RectTransform>();
        RectTransform iMTransform = InventoryMenu.GetComponent<RectTransform>();

        Vector3 originalPosition = menuTransform.anchoredPosition;
        Vector3 iMOP = iMTransform.anchoredPosition;

        if (!CharacterOpen)
        {
            menuTransform.anchoredPosition = new Vector2(5000, 5000);
            CharacterMenu.SetActive(true);
            yield return null;
            yield return null;
            CharacterMenu.SetActive(false);
        }

        if (!InventoryOpen)
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
