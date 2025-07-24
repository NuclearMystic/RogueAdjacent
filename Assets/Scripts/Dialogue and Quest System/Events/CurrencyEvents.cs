using System;
using UnityEngine;

public class CurrencyEvents  
{
    public event Action<float> onCurrencyGained;
    public void CurrencyGained(float amount)
    {
        if (onCurrencyGained != null)
        {
            onCurrencyGained(amount);
        }
    }

    public event Action<float> onCurrencyChange;
    public void CurrencyChange(float amount) {
        if (onCurrencyChange != null) { 
            onCurrencyChange(amount);
        }
    }

}
