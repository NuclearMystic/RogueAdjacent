using UnityEngine;

public static class DiceRoller
{
    public static int Roll(DiceType type, int amountOfDice = 1)
    {
        int result = 0;
        int max = GetMaxRollValue(type);

        for (int i = 0; i < amountOfDice; i++)
        {
            result += Random.Range(1, max + 1);
        }

        InGameConsole.Instance.SendMessageToConsole($"Rolled {result} on {amountOfDice} {type}!");
        return result;
    }

    private static int GetMaxRollValue(DiceType type)
    {
        switch (type)
        {
            case DiceType.D2: return 2;
            case DiceType.D4: return 4;
            case DiceType.D6: return 6;
            case DiceType.D8: return 8;
            case DiceType.D10: return 10;
            case DiceType.D12: return 12;
            case DiceType.D20: return 20;
            case DiceType.D100: return 100;
            default: return 6;
        }
    }


}

public enum DiceType
{
    None,
    D2,
    D4,
    D6,
    D8,
    D10,
    D12,
    D20,
    D100
}
