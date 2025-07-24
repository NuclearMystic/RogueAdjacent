using UnityEngine;

public class ConsoleTest : MonoBehaviour
{
    public KeyCode rollD2;
    public KeyCode rollD4;
    public KeyCode rollD6;
    public KeyCode rollD8;
    public KeyCode rollD10;
    public KeyCode rollD12;
    public KeyCode rollD20;
    public KeyCode rollD100;

    private void Update()
    {
        if (Input.GetKeyDown(rollD2))
        {
            DiceRoller.Roll(DiceType.D2, 1);
        }
        if (Input.GetKeyDown(rollD4))
        {
            DiceRoller.Roll(DiceType.D4, 1);
        }
        if (Input.GetKeyDown(rollD6))
        {
            DiceRoller.Roll(DiceType.D6, 1);
        }
        if (Input.GetKeyDown(rollD8))
        {
            DiceRoller.Roll(DiceType.D8, 1);
        }
        if (Input.GetKeyDown(rollD10))
        {
            DiceRoller.Roll(DiceType.D10, 1);
        }
        if (Input.GetKeyDown(rollD12))
        {
            DiceRoller.Roll(DiceType.D12, 1);          
        }
        if (Input.GetKeyDown(rollD20))
        {
            DiceRoller.Roll(DiceType.D20, 1);
        }
        if (Input.GetKeyDown(rollD100))
        {
            DiceRoller.Roll(DiceType.D100, 1);
        }

    }
}
