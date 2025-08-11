using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInitializer : MonoBehaviour
{
    private PlayerData playerData;
    private GameObject playerObject;

    public GameObject[] playerClassPrefabs;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            playerData = GameManager.Instance.playerData;
            AssignPlayerClass(playerData.PlayerClass);
        }

    }

    private void AssignPlayerClass(PlayerClass playerClass)
    {

        switch (playerClass)
        {
            case PlayerClass.None:
                Debug.LogWarning("Player class was not selected!");
                break;
            case PlayerClass.Archer:
                playerObject = playerClassPrefabs[0];
                break;
            case PlayerClass.Fighter:
                playerObject = playerClassPrefabs[1];
                break;
            case PlayerClass.Wizard:
                playerObject = playerClassPrefabs[2];
                break;
            default:
                Debug.LogError("Unhandled PlayerClass type!");
                break;
        }

        if (playerObject == null)
        {
            Debug.LogError("Selected playerObject is null!");
            return;
        }

        CreatePlayer();
    }

    private void CreatePlayer()
    {
        if (playerObject != null)
        {
            GameObject newPlayer = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(newPlayer);
            GameManager.Instance.SetPlayerObject(GameObject.FindWithTag("Player"));
        }
        else
        {
            Debug.LogError("Player object is not assigned!");
        }
    }
}
