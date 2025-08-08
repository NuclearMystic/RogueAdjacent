using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    bool optionsPanelVisible = false;

    public void StartGame()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void OptionsMenu()
    {
        if (optionsPanelVisible)
        {
            optionsPanel.SetActive(false);
            optionsPanelVisible = false;
        }
        else
        {
            optionsPanel.SetActive(true);
            optionsPanelVisible = true;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LoadGameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
