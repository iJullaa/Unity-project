using UnityEngine;

public class Door : MonoBehaviour
{
    public Inventory playerInventory; // Referencja do ekwipunku gracza
    public Item keyItem; // Przedmiot "Key", który jest wymagany do otwarcia drzwi
    public GameObject doorObject; // Obiekt drzwi, który ma zostaæ zniszczony

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerInventory.GetItemCount(keyItem) > 0)
        {
            playerInventory.RemoveItem(keyItem); // Usuñ klucz po u¿yciu
            Destroy(doorObject); // Usuñ drzwi
            Destroy(gameObject); // Usuñ trigger
        }
    }
}
