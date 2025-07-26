using UnityEngine;

public interface IPlayerClass
{
    void PerformAttack(PlayerStats stats, PlayerEquipmentManager equipment, TopDownCharacterController controller);
}