using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System;

public class InGameConsole : MonoBehaviour
{
    public static InGameConsole Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private int maxVisibleLines = 5;
    [SerializeField] private int maxMessages = 100;
    [SerializeField] private string defaultMessage = "Console Initialized";

    private List<string> messageList = new List<string>();
    private int startIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AddMessage(defaultMessage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ScrollUp();
        }
        else if (scroll < 0f)
        {
            ScrollDown();
        }
    }

    public void SendMessageToConsole(string message)
    {
        if (Instance != null)
        {
            Instance.AddMessage(message);
        }
        else
        {
            Debug.LogWarning("ConsoleWindow instance not found. Make sure it is added to the scene.");
        }
    }

    private void AddMessage(string message)
    {
        if (consoleText == null || backgroundPanel == null)
        {
            Debug.LogError("ConsoleText or BackgroundPanel is not assigned.");
            return;
        }

        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        messageList.Add($"[{timestamp}] {message}");

        if (messageList.Count > maxMessages)
        {
            messageList.RemoveAt(0);
        }

        // Auto-scroll to latest
        startIndex = Mathf.Max(0, messageList.Count - maxVisibleLines);
        UpdateConsoleText();
    }

    private void UpdateConsoleText()
    {
        consoleText.text = "";

        int endIndex = Mathf.Min(startIndex + maxVisibleLines, messageList.Count);
        for (int i = startIndex; i < endIndex; i++)
        {
            consoleText.text += messageList[i] + "\n";
        }
    }

    private void ScrollUp()
    {
        if (startIndex > 0)
        {
            startIndex--;
            UpdateConsoleText();
        }
    }

    private void ScrollDown()
    {
        if (startIndex + maxVisibleLines < messageList.Count)
        {
            startIndex++;
            UpdateConsoleText();
        }
    }

    public void ToggleConsole(bool isVisible)
    {
        backgroundPanel.gameObject.SetActive(isVisible);
    }
}
