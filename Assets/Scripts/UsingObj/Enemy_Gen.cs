using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gen : BaseObj
{
    [Header("������")]
    public int gen_sum; // �ܼ����ɶ��ٲ�
    public int gen_timer_set; // ������������һ��
    [SerializeField] private int gen_timer ; // ������������һ��

    // ����Enemy������
    public List<GameObject> enemyPool = new List<GameObject>(); // ���ɵĹֵ��б�

    public override void RhyAction()
    {
        Debug.Log("GEn");

        if(gen_timer <=0)
        {
            if(gen_sum >= 0 )
            {
                // �ڴ˴���������
                int tmp = Random.Range(0, enemyPool.Count);
                GameObject _newEne = GameObject.Instantiate(enemyPool[tmp], transform.position, Quaternion.identity);
                _newEne.GetComponent<Enemy>().setEnemyAlive(); //����
                                                               // ���ڴ˴�����

                gen_sum--;

                gen_timer = gen_timer_set; //�ָ�����
            }

        }
        else
        {
            gen_timer--; 
        }

    }

    protected override void ObjUpdate()
    {
        if(gen_sum <= 0)
        {
          //   GenOver();
        }
    }

    protected override void ObjStart()
    {
        base.ObjStart();
        this.isRhyObj = true;
    }

    private void GenOver()
    {
        //������ʧЧ
        Death();

    }
}
