using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float force = 10.0f;           // Normalna prędkość ruchu
    public float forceTurn = 10.0f;       // Siła obrotu
    public float jumpForce = 5.0f;        // Siła skoku

    public float sprintMultiplier = 1.5f; // Mnożnik prędkości przy sprincie
    public float crouchMultiplier = 0.5f; // Mnożnik prędkości przy kucaniu
    public float crawlMultiplier = 0.3f;  // Mnożnik prędkości przy czołganiu
    public float dashForce = 20.0f;       // Siła dasha
    public float dashDuration = 0.2f;     // Czas trwania dasha
    public float doubleTapTime = 0.3f;    // Maksymalny czas między dwoma naciśnięciami W dla dasha

    private bool isGrounded = true;       // Czy postać dotyka podłoża
    private bool isCrouching = false;     // Czy postać jest w pozycji kucającej
    private bool isCrawling = false;      // Czy postać jest w pozycji czołgającej się
    private bool isDashing = false;       // Czy postać wykonuje dash
    private int jumpCount = 0;            // Licznik skoków dla double jump

    private float lastWPressTime = -1f;   // Czas ostatniego naciśnięcia W
    private Vector3 originalScale;        // Oryginalna skala postaci
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);  // Skala przy kucaniu
    public Vector3 crawlScale = new Vector3(1, 0.3f, 1);   // Skala przy czołganiu

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale; // Zapisz oryginalną skalę postaci
    }

    void Update()
    {
        // Ustawienie aktualnej prędkości na podstawie sprintu, kucania i czołgania
        float currentForce = force;

        // Sprint (Shift) zwiększa prędkość, jeśli postać nie jest w pozycji kucającej ani czołgającej się
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isCrawling)
        {
            currentForce *= sprintMultiplier;
        }

        // Kucanie (Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            isCrawling = false;
            currentForce *= crouchMultiplier;
            transform.localScale = crouchScale;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            if (!isCrawling) // Przywróć oryginalny rozmiar, jeśli nie jest w pozycji czołgającej się
            {
                transform.localScale = originalScale;
            }
        }

        // Czołganie (C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrawling = !isCrawling; // Przełącz stan czołgania
            isCrouching = false;
            currentForce *= crawlMultiplier;

            // Zmiana skali na podstawie stanu czołgania
            if (isCrawling)
            {
                transform.localScale = crawlScale;
            }
            else
            {
                transform.localScale = originalScale;
            }
        }

        // Ruch do przodu (W) i wykrycie podwójnego naciśnięcia dla dasha
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Sprawdzenie, czy naciśnięcie jest podwójne
            if (Time.time - lastWPressTime < doubleTapTime && !isDashing)
            {
                StartCoroutine(Dash());
            }
            lastWPressTime = Time.time;
        }

        // Ruch do przodu (W) i do tyłu (S)
        if (Input.GetKey(KeyCode.W) && !isDashing)
        {
            rb.AddForce(transform.forward * currentForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * currentForce);
        }

        // Ruch w prawo (D) i w lewo (A)
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * currentForce);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * currentForce);
        }

        // Skok (Space) - double jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++; // Zwiększ licznik skoków
        }
    }

    // Funkcja dla dasha
    private IEnumerator Dash()
    {
        isDashing = true; // Ustaw, że postać wykonuje dash
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        // Poczekaj na zakończenie czasu dasha
        yield return new WaitForSeconds(dashDuration);

        isDashing = false; // Zakończ dash
    }

    // Funkcja sprawdzająca, czy postać dotknęła podłoża
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Postać dotyka podłoża
            jumpCount = 0;     // Zresetuj licznik skoków
        }
    }
}