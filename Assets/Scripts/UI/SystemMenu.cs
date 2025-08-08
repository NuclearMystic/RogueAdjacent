using UnityEngine;

public class SystemMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void ReturnToGame()
    {
        UIManager.Instance.ShowSystemMenu();
    }

    public void SaveGame()
    {
        gameManager.SaveGame();
        InGameConsole.Instance.SendMessageToConsole("Tried to save game.");
    }

    public void LoadGame()
    {
        gameManager.LoadGame();
        InGameConsole.Instance.SendMessageToConsole("Tried to load game.");
    }

    public void ToggleOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    public void QuitToMainMenu()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
