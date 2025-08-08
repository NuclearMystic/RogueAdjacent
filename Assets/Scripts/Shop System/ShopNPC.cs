using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopNPC : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueSO dialogueData;
    

    //private bool playerNear = false;
    

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GetComponent<DialogueManager>();
            dialogueData = dialogueManager.npcDia;
        }
    }

    private void Update()
    {
        
    }
    private void ShowDialogue()
    {

        string randomLine = dialogueData.proximityLines[Random.Range(0, dialogueData.proximityLines.Length)];
        dialogueManager.ShowText(randomLine);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           //playerNear = true;
            ShowDialogue();           
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           // playerNear = false;
        }
    }
}
