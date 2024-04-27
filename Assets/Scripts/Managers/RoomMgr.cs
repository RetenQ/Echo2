using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    // roomMgr不是传统意义上的MGR ， 实际上是控制房间是否可以出入的地方和条件
    public List<GameObject> enemys = new List<GameObject>(); // 所有敌人的总数
    public AudioSource passAudio;
    public List<GameObject> doors = new List<GameObject>(); // 用来阻拦的门

    public void RegisterObj(GameObject _obj)
    {
        // 一般用不到，因为正常情况都会提前加好
        enemys.Add(_obj);
    }

    public void ReomveObj(GameObject _obj)
    {
        enemys.Remove(_obj);
        if(enemys.Count == 0)
        {
            // 清空了
            PassRoom(); 
        }
    }

    public void EnterRoom()
    {
        // 玩家进入
        foreach(GameObject _door in doors)
        {
            _door.SetActive(true);
        }
    }

    public void PassRoom()
    {
        // 通过
        passAudio.Play();

        foreach (GameObject _door in doors)
        {
            _door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 玩家进行，开始
            EnterRoom();
        }
    }
}
