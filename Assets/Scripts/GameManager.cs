using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private GameObject playerObject;
    private Vector3 originalPosition; // Store the player's original position
    private string originalScene; // Store the player's original scene

    public void SetPlayerObject(GameObject playerObject) => this.playerObject = playerObject;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); // Ensure GameManager persists across scenes
        SetPlayerObject(GameObject.FindWithTag("Player"));
    }

    public void StartSceneLoad(string sceneName)
    {
        originalPosition = playerObject.transform.position; // Store the player's position
        originalScene = SceneManager.GetActiveScene().name; // Store the current scene name
        StartCoroutine(LoadSceneAndPlacePlayer(sceneName));
    }

    public IEnumerator LoadSceneAndPlacePlayer(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        //GetComponent<CombatManager>().detectionRadius = 0;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene is fully loaded, now find the spawn point
        Transform whereToPlacePlayer = GameObject.FindWithTag("SpawnPoint")?.transform;
        if (whereToPlacePlayer != null)
        {
            Vector3 spawnPosition = whereToPlacePlayer.position;

            // Move the player to the spawn point
            playerObject.transform.position = spawnPosition;

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
            //GetComponent<CombatManager>().detectionRadius = 5f;
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
}
