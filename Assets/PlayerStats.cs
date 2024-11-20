using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float attackPower = 10f;
    public float defense = 5f;

    public float healthRegenerationRate = 0f; // Czas trwania regeneracji w sekundach
    public float regenerationDuration = 5f;   // Czas trwania regeneracji w sekundach

    private float regenerationTimeElapsed = 0f; // Przechowuje czas, który upłynął podczas regeneracji

    public float currentHealth;

    private float baseMaxHealth;
    private float baseMoveSpeed;
    private float baseAttackPower;
    private float baseDefense;

    public bool dead = false;

    public HealthBar healthBar;

    void Start()
    {
        // Zapisujemy bazowe wartości, aby przy usunięciu bonusów można było do nich wrócić
        baseMaxHealth = maxHealth;
        baseMoveSpeed = moveSpeed;
        baseAttackPower = attackPower;
        baseDefense = defense;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }
        
        healthBar.SetHp(currentHealth/maxHealth);
        Debug.Log("Regen" + healthRegenerationRate);
    if (healthRegenerationRate > 0 && currentHealth < maxHealth && regenerationTimeElapsed < healthRegenerationRate)
        {
            Debug.Log("Regeneracja zdrowia");

            currentHealth += healthRegenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            regenerationTimeElapsed += Time.deltaTime;
        }
        else if (currentHealth >= maxHealth || regenerationTimeElapsed >= healthRegenerationRate)
        {
            Debug.Log("Regeneracja zakończona");
            regenerationTimeElapsed = 0f;
            healthRegenerationRate = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defense;
        if (finalDamage > 0)
            currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            dead = true;
        }
            
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Upewnij się, że zdrowie nie przekracza maksimum
    }

    // Metoda zapobiegająca spadkowi statystyk poniżej bazowych wartości
    public void ClampStats()
    {
        maxHealth = Mathf.Max(maxHealth, baseMaxHealth);
        moveSpeed = Mathf.Max(moveSpeed, baseMoveSpeed);
        attackPower = Mathf.Max(attackPower, baseAttackPower);
        defense = Mathf.Max(defense, baseDefense);
    }
}
