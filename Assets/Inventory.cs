using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerStats playerStats;
    private List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
        item.ApplyItem(playerStats); // Zastosuj bonus przedmiotu do statystyk postaci
        Debug.Log("Dodano przedmiot: " + item.itemName);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.RemoveItem(playerStats); // Usuń bonus przedmiotu ze statystyk
            Debug.Log("Usunięto przedmiot: " + item.itemName);
        }
    }
}
