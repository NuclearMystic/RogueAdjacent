using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public string dungeonToLoad;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameManager gameManger = FindAnyObjectByType<GameManager>();
            gameManger.StartSceneLoad(dungeonToLoad);
        }
    }
}
