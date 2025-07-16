using UnityEngine;

public static class DiceRoller
{
    public static int RollD2(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 3); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D2!");
        return result;
    }

    public static int RollD4(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 5); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D4!");
        return result;
    }

    public static int RollD6(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 7); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D6!");
        return result;
    }

    public static int RollD10(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 11); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D10!");
        return result;
    }

    public static int RollD12(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 13); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D12!");
        return result;
    }

    public static int RollD20(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 21); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D20!");
        return result;
    }

    public static int RollD100(int amountOfDice)
    {
        int result = 0;

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, 101); 
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} D100!");
        return result;
    }

}
