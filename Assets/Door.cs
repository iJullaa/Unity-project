using UnityEngine;

public class Door : MonoBehaviour
{
    public Inventory playerInventory; // Referencja do ekwipunku gracza
    public Item keyItem; // Przedmiot "Key", kt�ry jest wymagany do otwarcia drzwi
    public GameObject doorObject; // Obiekt drzwi, kt�ry ma zosta� zniszczony

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerInventory.GetItemCount(keyItem) > 0)
        {
            playerInventory.RemoveItem(keyItem); // Usu� klucz po u�yciu
            Destroy(doorObject); // Usu� drzwi
            Destroy(gameObject); // Usu� trigger
        }
    }
}
