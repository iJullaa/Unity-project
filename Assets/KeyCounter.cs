using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyCounter : MonoBehaviour
{
    public TMP_Text keyCounterText;
    public int keysCollected = 0;

    void Start()
    {
        keyCounterText.text = keysCollected.ToString();
    }

    public void AddKey()
    {
        keysCollected++;
        keyCounterText.text = keysCollected.ToString();
    }

    public void RemoveKey()
    {
        keysCollected--;
        keyCounterText.text = keysCollected.ToString();
    }
}
