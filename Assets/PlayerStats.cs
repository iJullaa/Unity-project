using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float attackPower = 10f;
    public float defense = 5f;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Ustaw zdrowie na maksimum na początku
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defense;
        if (finalDamage > 0)
            currentHealth -= finalDamage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // Logika śmierci gracza, np. restart poziomu
        Debug.Log("Player died.");
    }
}
