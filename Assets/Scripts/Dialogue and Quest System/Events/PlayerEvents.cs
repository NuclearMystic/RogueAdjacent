using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action<bool> onHealedAtInn;

    public void HealedAtInn(bool healedAtInn)
    {
        onHealedAtInn?.Invoke(healedAtInn);
    }
}
