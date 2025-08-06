using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject Crosshair;
    public GameObject ShopMenu;
    public GameObject SystemMenu;
    public GameObject LootBoxMenu;

    [Header("Runtime State (Read-Only)")]
    public bool LootBoxOpen => LootBoxMenu.activeInHierarchy;
    public bool InventoryOpen => InventoryMenu.activeInHierarchy;
    public bool CharacterOpen => CharacterMenu.activeInHierarchy;
    public bool SkillsMenuOpen => SkillsMenu.activeInHierarchy;
    public bool QuestMenuOpen => QuestMenu.activeInHierarchy;
    public bool ShopMenuOpen => ShopMenu.activeInHierarchy;
    public bool SystemMenuOpen => SystemMenu.activeInHierarchy;
    public UnityEvent updateSkillMenu;
    public void PauseGame() => Time.timeScale = 0f;
    public void UnPauseGame() => Time.timeScale = 1f;

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
        if (IsAnyMenuOpen())
        {
            if (Time.timeScale != 0f) PauseGame();
        }
        else
        {
            if (Time.timeScale != 1f) UnPauseGame();
        }

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
        updateSkillMenu?.Invoke();
    }

    public void ShowQuestMenu()
    {
        ToggleQuestMenu();
    }

    public void ShowSystemMenu()
    {
        ToggleSystemMenu();
    }


    private void ToggleCursor()
    {
        Crosshair.SetActive(IsAnyMenuOpen());
        Cursor.visible = false;
    }

    public bool IsAnyMenuOpen()
    {
        if (!refreshingMenus && (InventoryOpen || CharacterOpen || SkillsMenuOpen || QuestMenuOpen || ShopMenuOpen || SystemMenuOpen || LootBoxOpen))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ToggleSystemMenu()
    {
        SystemMenu.SetActive(!SystemMenu.activeInHierarchy);
    }

    private void ToggleQuestMenu()
    {
        QuestMenuManager.Instance.OpenMenu();
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
