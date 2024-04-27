using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{
    // roomMgr���Ǵ�ͳ�����ϵ�MGR �� ʵ�����ǿ��Ʒ����Ƿ���Գ���ĵط�������
    public List<GameObject> enemys = new List<GameObject>(); // ���е��˵�����
    public AudioSource passAudio;
    public List<GameObject> doors = new List<GameObject>(); // ������������

    public void RegisterObj(GameObject _obj)
    {
        // һ���ò�������Ϊ�������������ǰ�Ӻ�
        enemys.Add(_obj);
    }

    public void ReomveObj(GameObject _obj)
    {
        enemys.Remove(_obj);
        if(enemys.Count == 0)
        {
            // �����
            PassRoom(); 
        }
    }

    public void EnterRoom()
    {
        // ��ҽ���
        foreach(GameObject _door in doors)
        {
            _door.SetActive(true);
        }
    }

    public void PassRoom()
    {
        // ͨ��
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
            // ��ҽ��У���ʼ
            EnterRoom();
        }
    }
}
