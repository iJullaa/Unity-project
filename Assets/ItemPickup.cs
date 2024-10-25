using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter(Collider other)
    {
        Inventory playerInventory = other.GetComponent<Inventory>();
        if (playerInventory != null)
        {
            playerInventory.AddItem(item);
            Destroy(gameObject); // Usuń przedmiot z mapy po podniesieniu
        }
    }
}
