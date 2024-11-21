using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingModel : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Szybkość obrotu
    private Quaternion targetRotation;

    public GameObject player;
    public Camera mainCamera; // Kamera gracza

    void Start()
    {
        targetRotation = transform.rotation;
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Pobierz główną kamerę, jeśli nie została przypisana
        }
    }

    void Update()
    {
        if (player.GetComponent<PlayerStats>().dead == false)
        {
            // Pobieranie wartości osi ruchu
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Jeśli jest jakikolwiek ruch, obliczamy nową rotację
            if (horizontal != 0 || vertical != 0)
            {
                // Obliczanie kierunku na podstawie kamery
                Vector3 forward = mainCamera.transform.forward;
                forward.y = 0; // Wykluczamy zmiany w osi Y
                forward.Normalize();

                Vector3 right = mainCamera.transform.right;
                right.y = 0; // Wykluczamy zmiany w osi Y
                right.Normalize();

                // Obliczanie kierunku, w którym gracz chce się poruszać
                Vector3 moveDirection = forward * vertical + right * horizontal;

                // Obliczanie kąta rotacji
                float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, angle, 0);  // Ustawiamy rotację na osi Y
            }

            // Interpolacja pomiędzy bieżącą rotacją a docelową rotacją
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}