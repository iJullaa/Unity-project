using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject[] dropItems; // Array of items that can drop
    public float dropChance = 0.5f; // Chance to drop an item (e.g. 50%)

    private Vector3 position;

    void Start()
    {
        position = transform.position;
    }

    public void DropRandomItem(Vector3 position)
    {
        if (Random.value <= dropChance) // Random chance to drop an item
        {
            // Pick a random item from the array and instantiate it at the saved position
            GameObject itemToDrop = dropItems[Random.Range(0, dropItems.Length)];
            Instantiate(itemToDrop, position, Quaternion.identity); // Instantiate the item at the saved position
            itemToDrop.transform.position = position; // Set the position of the item
            itemToDrop.SetActive(true); // Activate the item
        }
    }

    public void OnBombExplosion()
    {
        StartCoroutine(DestroyRock());
    }

    private IEnumerator DestroyRock()
    {
        // Save the position of the rock before destroying it
        Vector3 rockPosition = position;
        // Drop the item at the saved position
        DropRandomItem(rockPosition);

        // Wait until the next frame (to allow item drop to complete before destroy)
        yield return null;

        // Destroy the rock after the item has been dropped
        Destroy(this.gameObject);
    }
}
