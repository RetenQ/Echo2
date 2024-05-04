using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    // roomMgr不是传统意义上的MGR ， 实际上是控制房间是否可以出入的地方和条件
    public List<GameObject> enemys = new List<GameObject>(); // 所有敌人的总数
    public AudioSource passAudio;
    public List<GameObject> doors = new List<GameObject>(); // 用来阻拦的门

    public List<GameObject> enenmyGen= new List<GameObject>(); //激活的生成器

    public int EnemySum; //敌人总数

    [Header("临时测试用")]
    public bool isBoss = false;
    public string nextScene; 
    public float delayTime; 

    private void Start()
    {
        IniEnemyInList();
    }
    public void RegisterObj(GameObject _obj)
    {
        // 一般用不到，因为正常情况都会提前加好
        enemys.Add(_obj);
    }

    public void ReomveObj(GameObject _obj)
    {
        EnemySum--;

        enemys.Remove(_obj);
        if(enemys.Count == 0 && EnemySum <= 0)
        {
            // 清空了
            PassRoom(); 
        }
    }

    public void IniEnemyInList()
    {
        foreach(GameObject _obj in enemys)
        {
            _obj.GetComponent<BaseObj>().setAlive();
        }
    }

    public void IniEnemy(GameObject _obj)
    {
        // 添加的敌人要被调用，然后设置每个敌人的roommanager和敌人对于
        _obj.GetComponent<Enemy>().setRoomManager(gameObject); 
    }

    public void EnterRoom()
    {
        // 玩家进入
        if (doors.Count > 0)
        {
            foreach (GameObject _door in doors)
            {
                _door.SetActive(true);
            }
        }
        else
        {
            Debug.Log("List_doors is empty");
        }

        if (enenmyGen.Count > 0)
        {
            foreach (GameObject _gen in enenmyGen)
            {
                _gen.GetComponent<BaseObj>().setAlive();
            }

        }
        else
        {
            Debug.Log("enenmyGen is empty");

        }
    }

        public void PassRoom()
    {
        // 通过

        RhythmMgr.GetInstance().StopAllMusicRhy();

        passAudio.Play();

        foreach (GameObject _door in doors)
        {
            _door.SetActive(false);
        }

        if (isBoss)
        {
            Invoke("IntoNextScene", delayTime);
        }
    }

    private void IntoNextScene()
    {
        GameManager.GetInstance().LoadNextScene(nextScene);
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
