using UnityEngine;

public class SpikeColorChange : MonoBehaviour
{
    public Color defaultColor = Color.gray;   // Domyœlny kolor kolców
    public Color triggeredColor = Color.red; // Kolor po aktywacji
    public float colorChangeDuration = 1f;   // Czas powrotu do domyœlnego koloru
    private Material spikeMaterial;          // Referencja do materia³u kolców

    void Start()
    {
        // Pobranie materia³u z renderer'a obiektu
        spikeMaterial = GetComponent<Renderer>().material;
        spikeMaterial.color = defaultColor; // Ustawienie koloru pocz¹tkowego
    }

    private void OnTriggerEnter(Collider other)
    {
        // SprawdŸ, czy gracz wszed³ na kolce
        if (other.CompareTag("Player"))
        {
            // Zmieñ kolor na aktywowany
            ChangeColor(triggeredColor);

            // Mo¿esz dodaæ dodatkowe efekty, np. zadawanie obra¿eñ graczowi
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gdy gracz opuszcza kolce, przywróæ domyœlny kolor
        if (other.CompareTag("Player"))
        {
            ChangeColor(defaultColor);
        }
    }

    private void ChangeColor(Color newColor)
    {
        // Zmiana koloru materia³u
        spikeMaterial.color = newColor;

        // Opcjonalnie: interpolacja koloru w czasie
        if (colorChangeDuration > 0)
        {
            StopAllCoroutines(); // Zatrzymaj inne interpolacje, jeœli ju¿ dzia³aj¹
            StartCoroutine(LerpColor(newColor));
        }
    }

    private System.Collections.IEnumerator LerpColor(Color targetColor)
    {
        Color startColor = spikeMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeDuration)
        {
            // Interpolacja miêdzy bie¿¹cym a docelowym kolorem
            spikeMaterial.color = Color.Lerp(startColor, targetColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spikeMaterial.color = targetColor; // Na koniec ustaw ostateczny kolor
    }
}
