using System.Collections;
using UnityEngine;

public class PlayerSpikeDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;  // Obrażenia na sekundę
    public float damageInterval = 1f;    // Odstęp czasu między zadawaniem obrażeń
    private bool isOnSpikes = false;     // Flaga czy gracz stoi na kolcach
    private PlayerStats playerStats;     // Referencja do statystyk gracza

    public Animator ani; // Animator postaci


    void Start()
    {
        // Pobierz komponent PlayerStats, który przechowuje zdrowie gracza
        playerStats = GetComponent<PlayerStats>();
        playerStats.currentHealth = playerStats.maxHealth; // Ustaw zdrowie na maksimum na początku
    }
    
    void Update()
    {
        if (isOnSpikes && playerStats.currentHealth > 0)
        {
            ani.SetTrigger("Hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdź, czy gracz wszedł na obiekt z tagiem "Spikes"
        if (other.CompareTag("Spikes"))
        {
            isOnSpikes = true;
            if (playerStats.currentHealth > 0){
                StartCoroutine(DamageOverTime());  // Rozpocznij zadawanie obrażeń w czasie
            }
            else
            {
                isOnSpikes = false;
            }
            Debug.Log("Gracz wszedł na kolce.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sprawdź, czy gracz opuszcza obiekt z tagiem "Spikes"
        if (other.CompareTag("Spikes"))
        {
            isOnSpikes = false;
            Debug.Log("Gracz opuścił kolce.");
        }
    }

    private IEnumerator DamageOverTime()
    {
        // Dopóki gracz jest na kolcach, zadawaj obrażenia co określony odstęp czasu
        while (isOnSpikes)
        {
            if (playerStats != null)
            {
                if (playerStats.currentHealth <= 0)
                {
                    isOnSpikes = false;
                    break;
                }
                playerStats.TakeDamage(damagePerSecond * damageInterval);  // Zadaj obrażenia
                Debug.Log("Zdrowie gracza: " + playerStats.currentHealth);
            }
            yield return new WaitForSeconds(damageInterval);  // Odczekaj przed kolejnymi obrażeniami
        }
    }
}
