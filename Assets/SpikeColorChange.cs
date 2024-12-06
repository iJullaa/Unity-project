using UnityEngine;

public class SpikeColorChange : MonoBehaviour
{
    public Color defaultColor = Color.gray;   // Domy�lny kolor kolc�w
    public Color triggeredColor = Color.red; // Kolor po aktywacji
    public float colorChangeDuration = 1f;   // Czas powrotu do domy�lnego koloru
    private Material spikeMaterial;          // Referencja do materia�u kolc�w

    void Start()
    {
        // Pobranie materia�u z renderer'a obiektu
        spikeMaterial = GetComponent<Renderer>().material;
        spikeMaterial.color = defaultColor; // Ustawienie koloru pocz�tkowego
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawd�, czy gracz wszed� na kolce
        if (other.CompareTag("Player"))
        {
            // Zmie� kolor na aktywowany
            ChangeColor(triggeredColor);

            // Mo�esz doda� dodatkowe efekty, np. zadawanie obra�e� graczowi
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gdy gracz opuszcza kolce, przywr�� domy�lny kolor
        if (other.CompareTag("Player"))
        {
            ChangeColor(defaultColor);
        }
    }

    private void ChangeColor(Color newColor)
    {
        // Zmiana koloru materia�u
        spikeMaterial.color = newColor;

        // Opcjonalnie: interpolacja koloru w czasie
        if (colorChangeDuration > 0)
        {
            StopAllCoroutines(); // Zatrzymaj inne interpolacje, je�li ju� dzia�aj�
            StartCoroutine(LerpColor(newColor));
        }
    }

    private System.Collections.IEnumerator LerpColor(Color targetColor)
    {
        Color startColor = spikeMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeDuration)
        {
            // Interpolacja mi�dzy bie��cym a docelowym kolorem
            spikeMaterial.color = Color.Lerp(startColor, targetColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spikeMaterial.color = targetColor; // Na koniec ustaw ostateczny kolor
    }
}
