using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    [Tooltip("Reference to the Input Field UI element")]
    [SerializeField] private TMP_InputField nameInputField;
    [Tooltip("Game manger prefab to spawn before starting the game. This will persist between scenes and hold player data.")]
    [SerializeField] private GameObject gameManagerPrefab;

    // player data creation
    private PlayerData playerData;
    private string playerName;
    private PlayerClass playerClass;

    // confirmation panel
    public GameObject confirmationPanel;
    public TMP_Text confirmText;

    private void Start()
    {
        nameInputField.onEndEdit.AddListener(EnterPlayerName);
    }

    public void EnterPlayerName(string name)
    {
        playerName = name;
        Debug.Log("Player Name set to: " + playerName);
    }

    public void SelectClass(int value)
    {
        switch (value)
        {
            case 1:
                playerClass = PlayerClass.Archer;
                Debug.Log("Archer class selected!");
                break;
            case 2:
                playerClass = PlayerClass.Fighter;
                Debug.Log("Fighter class selected!");
                break;
            case 3:
                playerClass = PlayerClass.Wizard;
                Debug.Log("Wizard class selected!");
                break;
            default:
                Debug.LogWarning("Invalid class selection.");
                break;
        }
        Debug.Log($"{playerClass} has been set to {playerClass.GetType()}");
    }

    public void ConfirmSelections()
    {
        InitializePlayer();
        Debug.Log($"Player initialized with the name {playerData.PlayerName} and the class of {playerData.PlayerClass}!");

        if (string.IsNullOrWhiteSpace(playerData.PlayerName) || playerData.PlayerClass == PlayerClass.None)
        {
            if (string.IsNullOrWhiteSpace(playerData.PlayerName))
            {
                Debug.Log("Please enter a name first.");
                return;
            }
            else if (playerData.PlayerClass == PlayerClass.None)
            {
                Debug.Log("Please select a class first.");
                return;
            }
        }
        else if (playerData.PlayerName != null && playerData.PlayerClass != PlayerClass.None)
        {
            confirmText.text = $"Are you sure you wish to be a {playerClass} named {playerName}?";
            confirmationPanel.SetActive(true);
        }
    }

    public void InitializePlayer()
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.PlayerName = this.playerName;
        playerData.PlayerClass = this.playerClass;
    }

    public void StartGame()
    {
        Instantiate(gameManagerPrefab);
        GameManager.Instance.SetPlayerData(playerData);
        SceneManager.LoadScene("Town");
    }
}

[System.Serializable]
public enum PlayerClass
{
    None,
    Archer,
    Fighter,
    Wizard
}
