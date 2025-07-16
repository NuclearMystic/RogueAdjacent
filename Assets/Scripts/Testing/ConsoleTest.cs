using UnityEngine;

public class ConsoleTest : MonoBehaviour
{
    public KeyCode rollD2;
    public KeyCode rollD4;
    public KeyCode rollD6;
    public KeyCode rollD10;
    public KeyCode rollD12;
    public KeyCode rollD20;
    public KeyCode rollD100;

    private void Update()
    {
        if (Input.GetKeyDown(rollD2))
        {
            DiceRoller.RollD2(1);
        }
        if (Input.GetKeyDown(rollD4))
        {
            DiceRoller.RollD4(1);
        }
        if (Input.GetKeyDown(rollD6))
        {
            DiceRoller.RollD6(1);
        }
        if (Input.GetKeyDown(rollD10))
        {
            DiceRoller.RollD10(1);
        }
        if (Input.GetKeyDown(rollD12))
        {
            DiceRoller.RollD12(1);          
        }
        if (Input.GetKeyDown(rollD20))
        {
            DiceRoller.RollD20(1);
        }
        if (Input.GetKeyDown(rollD100))
        {
            DiceRoller.RollD100(1);
        }

    }
}
