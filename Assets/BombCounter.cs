using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombCounter : MonoBehaviour
{
    public TMP_Text bombCounterText;
    public int bombsCollected = 0;

    void Start()
    {
        bombCounterText.text = bombsCollected.ToString();
    }

    public void AddBomb()
    {
        bombsCollected++;
        bombCounterText.text = bombsCollected.ToString();
    }

    public void RemoveBomb()
    {
        bombsCollected--;
        bombCounterText.text = bombsCollected.ToString();
    }
}
