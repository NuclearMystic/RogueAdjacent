using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        CreateAndLoadPlayerData();
    }

    public void CreateAndLoadPlayerData()
    {
        PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.LoadData();
        gameManager.SetPlayerData(playerData);
        FindAndLoadSavedScene();
    }

    public void FindAndLoadSavedScene()
    {
        string sceneToLoad = gameManager.playerData.CurrentScene;
        Debug.Log(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
