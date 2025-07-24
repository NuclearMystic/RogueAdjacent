using UnityEngine;
using System;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public CurrencyEvents currencyEvents;
    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public InteractionEvents interactionEvents;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Destroying new events manager");
            Destroy(this);
            return;
            
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        currencyEvents = new CurrencyEvents();
        questEvents = new QuestEvents();
        miscEvents = new MiscEvents();
        interactionEvents = new InteractionEvents();
    }
}