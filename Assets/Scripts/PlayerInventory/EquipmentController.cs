using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) HandleNumberKey(KeyCode.Alpha1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) HandleNumberKey(KeyCode.Alpha2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) HandleNumberKey(KeyCode.Alpha3);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) HandleNumberKey(KeyCode.Alpha4);
    }

    void HandleNumberKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha1:
                PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(1);
                break;
            case KeyCode.Alpha2:
                PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(2);
                break;
            case KeyCode.Alpha3:
                PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(3);
                break;
            case KeyCode.Alpha4:
                PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(4);
                break;
        }
    }
}
