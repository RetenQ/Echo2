using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gen : BaseObj
{
    [Header("生怪器")]
    public int gen_sum; // 总计生成多少波
    public int gen_timer_set; // 几个拍子生成一次
    [SerializeField] private int gen_timer ; // 几个拍子生成一次

    // 生成Enemy的物体
    public List<GameObject> enemyPool = new List<GameObject>(); // 生成的怪的列表

    public override void RhyAction()
    {
        Debug.Log("GEn");

        if(gen_timer <=0)
        {
            if(gen_sum >= 0 )
            {
                // 在此处进行生成
                int tmp = Random.Range(0, enemyPool.Count);
                GameObject _newEne = GameObject.Instantiate(enemyPool[tmp], transform.position, Quaternion.identity);
                _newEne.GetComponent<Enemy>().setEnemyAlive(); //启动
                                                               // 就在此处生成

                gen_sum--;

                gen_timer = gen_timer_set; //恢复计数
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
        //生成器失效
        Death();

    }
}
