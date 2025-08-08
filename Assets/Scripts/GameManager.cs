using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using AASave;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerData playerData { get; private set; }

    private string playerName;

    private GameObject playerObject;
    private Vector3 originalPosition; 
    private string originalScene;

    public string GetPlayerName()
    {
        return playerData.PlayerName;
    }
    public PlayerClass GetPlayerClass()
    {
        return playerData.PlayerClass;
    }

    public void SetPlayerObject(GameObject playerObject) => this.playerObject = playerObject;
    public void SetPlayerData(PlayerData playerData) => this.playerData = playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("destroy extra game manager");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //if (playerObject == null)
        //{
        //    SetPlayerObject(GameObject.FindWithTag("Player"));
        //}
        
    }

    public void StartSceneLoad(string sceneName)
    {
        originalPosition = playerObject.transform.position;
        originalScene = SceneManager.GetActiveScene().name; 
        StartCoroutine(LoadSceneAndPlacePlayer(sceneName));
    }

    public IEnumerator LoadSceneAndPlacePlayer(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene is fully loaded, now find the spawn point
        Transform whereToPlacePlayer = GameObject.FindWithTag("SpawnPoint")?.transform;
        Debug.Log($"Placing player at {whereToPlacePlayer.position}");
        if (whereToPlacePlayer != null)
        {
            Vector3 spawnPosition = whereToPlacePlayer.position;

            // Move the player to the spawn point
            playerObject.transform.position = spawnPosition;
            Debug.Log($"Player moved to {spawnPosition}");

            // Reset Rigidbody velocities
            Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Update the player's targetCellPosition
            //Player player = playerObject.GetComponent<Player>();
            //player.targetCellPosition = player.tilemap.WorldToCell(spawnPosition);

            // Optionally, wait for an additional frame to ensure all initializations are done
            yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("SpawnPoint not found in the new scene.");
        }
    }

    public void WarpBackToOriginalPosition()
    {
        StartCoroutine(LoadOriginalSceneAndPlacePlayer());
    }

    private IEnumerator LoadOriginalSceneAndPlacePlayer()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(originalScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene is fully loaded, now place the player at the original position
        playerObject.transform.position = originalPosition + Vector3.down;

        // Reset Rigidbody velocities
        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Update the player's targetCellPosition
        //Player player = playerObject.GetComponent<Player>();
        //player.targetCellPosition = player.tilemap.WorldToCell(originalPosition);
    }

    public void SaveGame()
    {
        playerData.SaveData();
    }

    public void LoadGame()
    {
        // SceneManager.LoadScene("LoadGameScene");
    }
}
