using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_NextRoom : MonoBehaviour
{
    // 检测到Player的时候触发
    public string toScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBase>().StopPlayer(); // 锁定player

            LoadScene(toScene); 
        }
    }

    private void LoadScene(string _des)
    {
        SceneManager.LoadScene(_des);
    }
}
