using System;
using UnityEngine;

public class EnemyEvents
{
    public event Action<string> onEnemyKilled;

    public void EnemyKilled(string enemyName)
    {
        onEnemyKilled?.Invoke(enemyName);
    }
}
