using UnityEngine;

public class SystemMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    public void ReturnToGame()
    {
        UIManager.Instance.ShowSystemMenu();
    }

    public void SaveGame()
    {
        InGameConsole.Instance.SendMessageToConsole("Not yet implemented.");
    }

    public void LoadGame()
    {
        InGameConsole.Instance.SendMessageToConsole("Not yet implemented.");
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
