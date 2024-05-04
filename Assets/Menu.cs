using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string toNext; 

    public void Load()
    {
        SceneManager.LoadScene(toNext);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
