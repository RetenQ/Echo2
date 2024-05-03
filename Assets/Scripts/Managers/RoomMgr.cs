using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    // roomMgr���Ǵ�ͳ�����ϵ�MGR �� ʵ�����ǿ��Ʒ����Ƿ���Գ���ĵط�������
    public List<GameObject> enemys = new List<GameObject>(); // ���е��˵�����
    public AudioSource passAudio;
    public List<GameObject> doors = new List<GameObject>(); // ������������

    public List<GameObject> enenmyGen= new List<GameObject>(); //�����������

    public int EnemySum; //��������

    [Header("��ʱ������")]
    public bool isBoss = false;
    public string nextScene; 
    public float delayTime; 

    private void Start()
    {
        IniEnemyInList();
    }
    public void RegisterObj(GameObject _obj)
    {
        // һ���ò�������Ϊ�������������ǰ�Ӻ�
        enemys.Add(_obj);
    }

    public void ReomveObj(GameObject _obj)
    {
        EnemySum--;

        enemys.Remove(_obj);
        if(enemys.Count == 0 && EnemySum <= 0)
        {
            // �����
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
        // ��ӵĵ���Ҫ�����ã�Ȼ������ÿ�����˵�roommanager�͵��˶���
        _obj.GetComponent<Enemy>().setRoomManager(gameObject); 
    }

    public void EnterRoom()
    {
        // ��ҽ���
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
        // ͨ��

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
            // ��ҽ��У���ʼ
            EnterRoom();
        }
    }
}
