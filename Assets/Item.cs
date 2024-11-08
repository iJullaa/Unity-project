using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public float healthBonus = 0f;
    public float moveSpeedBonus = 0f;
    public float attackPowerBonus = 0f;
    public float defenseBonus = 0f;
    public float healthRegenerationBonus = 0f;


    public void ApplyItem(PlayerStats playerStats)
    {
        playerStats.maxHealth += healthBonus;
        playerStats.moveSpeed += moveSpeedBonus;
        playerStats.attackPower += attackPowerBonus;
        playerStats.defense += defenseBonus;
        playerStats.healthRegenerationRate += healthRegenerationBonus;

    }

    public void RemoveItem(PlayerStats playerStats)
    {
        playerStats.maxHealth -= healthBonus;
        playerStats.moveSpeed -= moveSpeedBonus;
        playerStats.attackPower -= attackPowerBonus;
        playerStats.defense -= defenseBonus;
        playerStats.healthRegenerationRate -= healthRegenerationBonus;

        playerStats.ClampStats();
    }
}
