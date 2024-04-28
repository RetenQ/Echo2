using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_NextRoom : MonoBehaviour
{
    // ��⵽Player��ʱ�򴥷�
    public string toScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBase>().StopPlayer(); // ����player

            LoadScene(toScene); 
        }
    }

    private void LoadScene(string _des)
    {
        SceneManager.LoadScene(_des);
    }
}
