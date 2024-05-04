using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DemoEnd : MonoBehaviour
{
    public string toNext; // "MainRoom"

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(toNext);
        }
    }
}
