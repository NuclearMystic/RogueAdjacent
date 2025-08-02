using UnityEngine;

public class PlayerData : ScriptableObject
{
    [SerializeField] private string playerName;
    [SerializeField] private PlayerClass playerClass;

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
}
