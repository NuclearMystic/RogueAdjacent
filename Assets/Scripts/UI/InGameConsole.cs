using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InGameConsole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static InGameConsole Instance { get; private set; }

    [SerializeField] private RectTransform contentArea;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private ScrollRect scrollRect;

    private List<string> messageHistory = new List<string>();
    private Queue<GameObject> messageObjects = new Queue<GameObject>();

    private bool isMouseOver = false;
    private int maxMessages = 100;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SendMessageToConsole(string msg)
    {
        messageHistory.Add(msg);

        if (messageObjects.Count >= maxMessages)
        {
            GameObject oldest = messageObjects.Dequeue();
            Destroy(oldest);
        }

        GameObject newMsgObj = Instantiate(messagePrefab, contentArea);
        Text txt = newMsgObj.GetComponent<Text>();
        if (txt != null)
        {
            txt.text = msg;
        }

        messageObjects.Enqueue(newMsgObj);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f; // Focus on newest
    }

    void Update()
    {
        if (isMouseOver)
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0)
            {
                float newPos = scrollRect.verticalNormalizedPosition + scrollDelta * 0.2f;
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPos);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}
