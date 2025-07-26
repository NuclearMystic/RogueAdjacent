using UnityEngine;

public class PlayerCurrencyManager : MonoBehaviour
{
    public static PlayerCurrencyManager Instance { get; private set; }

    [SerializeField]
    private float startingCurrency;

    public float currentCurrency { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentCurrency = startingCurrency;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.currencyEvents.onCurrencyGained += CurrencyGained;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.currencyEvents.onCurrencyGained -= CurrencyGained;
    }

    private void Start()
    {
        GameEventsManager.instance.currencyEvents.CurrencyChange(GetCurrency());
    }

    public bool CanAfford(float ip)
    {
        if (ip <= currentCurrency)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    public void Spend(float ip)
    {
        currentCurrency -= ip;
        GameEventsManager.instance.currencyEvents.CurrencyChange(currentCurrency);
    }

    public float GetCurrency()
    {
        return currentCurrency;
    }

    public void CurrencyGained(float ic)
    {
        currentCurrency += ic;
        GameEventsManager.instance.currencyEvents.CurrencyChange(currentCurrency);
    }

}
