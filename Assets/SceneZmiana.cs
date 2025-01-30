using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneZmiana : MonoBehaviour
{
    public void ZmienScene(string nazwaSceny)
    {
        SceneManager.LoadScene(nazwaSceny);
    }
    public void Wyjdz()
    {
        Application.Quit();
    }
}
