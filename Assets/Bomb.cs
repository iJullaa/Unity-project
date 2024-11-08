using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 2.0f;      // Promień eksplozji
    public float explosionDelay = 3.0f;       // Czas do eksplozji
    public GameObject explosionEffect;        // Efekt eksplozji (np. animacja)

    private bool hasExploded = false;         // Flaga mówiąca, czy bomba już wybuchła

    void Start()
    {
            Debug.Log("Bomba ustawiona do eksplozji.");
            this.gameObject.SetActive(true);
            StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        // Odczekaj ustalony czas
        yield return new WaitForSeconds(explosionDelay);

        // Zainicjuj wybuch, jeśli bomba jeszcze nie wybuchła
        if (!hasExploded)
        {
            Debug.Log("Zainicjuj wybuch");
            Explode();
        }
    }

    // Funkcja wykonująca wybuch
    private void Explode()
    {
        hasExploded = true; // Set the flag that the bomb has exploded

        // Create the explosion effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Debug.Log("Bomba eksploduje, generując efekt eksplozji.");

        // Get the ParticleSystem component from the explosion effect
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();


        // Find all objects within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // Check if the object is a rock
            Rock rock = hit.GetComponent<Rock>();
            if (rock != null)
            {
                Debug.Log("Wybuch - wykryto kamień, który zostanie zniszczony.");
                rock.OnBombExplosion(); // Drop item and destroy rock
            }
            else
            {
                Debug.Log("Wybuch - wykryto obiekt, który nie jest kamieniem: " + hit.name);
            }
        }

        // Destroy the bomb itself after the explosion
        Debug.Log("Bomba zostaje zniszczona.");
        Destroy(gameObject);
        Destroy(explosion, particleSystem.main.duration); // Destroy the explosion effect after it finishes playing
    }
}
