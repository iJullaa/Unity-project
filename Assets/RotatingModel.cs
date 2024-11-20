using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingModel : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Szybkość obrotu
    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Pobieranie wartości osi ruchu
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Określenie docelowej rotacji na podstawie osi ruchu
        if (horizontal != 0 || vertical != 0)  // Sprawdzenie, czy nastąpił ruch
        {
            float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg; // Obliczanie kąta
            targetRotation = Quaternion.Euler(0, angle, 0);  // Ustawienie rotacji docelowej
        }

        // Interpolacja pomiędzy bieżącą rotacją a docelową rotacją
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
