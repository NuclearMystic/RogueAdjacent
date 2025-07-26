using TMPro;
using UnityEngine;
using UnityEditor.Rendering;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueSO npcDia;
    [SerializeField] Canvas dialogueBox;
    [SerializeField] TextMeshProUGUI dialogue;
    public float timePerCharacter = 0.05f;

    private RectTransform canvasRect;

    private void Start()
    {
        canvasRect = dialogueBox.GetComponent<RectTransform>();
    }

    public void ShowText(string incomingText)
    {
        dialogue.text = incomingText;
        //ResizeCanvasToFitText();
        dialogue.gameObject.SetActive(true);
        StartCoroutine(DelayHideText(incomingText));
    }

    public void HideText()
    {
        dialogue.gameObject.SetActive(false);
    }

    private void ResizeCanvasToFitText()
    {
        float textWidth = dialogue.preferredWidth;
        float textHeight = dialogue.preferredHeight;

        float padding = 20f;

        canvasRect.sizeDelta = new Vector2(textWidth, textHeight + padding);
    }

    private IEnumerator DelayHideText(string inText)
    {
        // Calculate the total time to show the dialogue based on the length of the text
        float totalTime = inText.Length * timePerCharacter;

        // Gradually show each character of the dialogue
        for (int i = 0; i <= inText.Length; i++)
        {
            dialogue.text = inText.Substring(0, i);  // Show the text up to the current character
            yield return new WaitForSeconds(timePerCharacter);  // Wait for the next character
        }

        // Wait until the full dialogue has been displayed
        yield return new WaitForSeconds(totalTime * 2);

        // Optionally, you can hide the dialogue box after the text has been displayed
        HideText();
    }
}
