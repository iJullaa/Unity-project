using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerStats playerStats;
    private List<Item> items = new List<Item>();
    public KeyCounter keyCounter;
    public BombCounter bombCounter;
    public static bool hasKey = false;  // Flaga określająca, czy gracz ma klucz

    public void AddItem(Item item)
    {
        items.Add(item);
        item.ApplyItem(playerStats); // Zastosuj bonus przedmiotu do statystyk postaci
        Debug.Log("Dodano przedmiot: " + item.itemName);
        if (item.itemName == "Key")
        {
            keyCounter.AddKey();
        }
        else if (item.itemName == "Bomb")
        {
            bombCounter.AddBomb();
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.RemoveItem(playerStats); // Usuń bonus przedmiotu ze statystyk
            Debug.Log("Usunięto przedmiot: " + item.itemName);
        }
        if (item.itemName == "Key")
        {
            keyCounter.RemoveKey();
        }
        else if (item.itemName == "Bomb")
        {
            bombCounter.RemoveBomb();
        }
    }

    public int GetItemCount(Item item)
    {
        int count = 0;

        foreach (Item invItem in items)
        {
            if (invItem == item)
            {
                count++;
            }
        }

        return count;
    }

    
}
