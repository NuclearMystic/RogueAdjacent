using AASave;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : ScriptableObject
{
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private string playerName;
    [SerializeField] private PlayerClass playerClass;
    [SerializeField] private string currentScene;

    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }

    public PlayerClass PlayerClass
    {
        get => playerClass;
        set => playerClass = value;
    }

    public string CurrentScene
    {
        get => currentScene;
        set => SceneManager.GetActiveScene();
    }

    public void SaveData()
    {
        if (saveSystem == null)
        {
            saveSystem = GameManager.Instance.gameObject.GetComponent<SaveSystem>();
        }

        saveSystem.Save("currentScene", currentScene);
        saveSystem.Save("playerName", playerName);
        saveSystem.Save("playerClass", (int)playerClass);
    }

    public void LoadData()
    {
        if (saveSystem == null)
        {
            saveSystem = GameManager.Instance.gameObject.GetComponent<SaveSystem>();
        }

        currentScene = saveSystem.Load("currentScene").AsString();
        Debug.Log($"Saved scene = {currentScene}");
        PlayerName = saveSystem.Load("playerName").AsString();
        PlayerClass = (PlayerClass)saveSystem.Load("playerClass").AsInt();
    }
}
