using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceMgr : SingletonMono<ChanceMgr>
{
    public Dictionary<double , double> P_CDic = new Dictionary<double, double>();
    public Dictionary<string, CValueObj> Act_CVObjeDic = new Dictionary<string, CValueObj>(); 

    // Start is called before the first frame update
    void Start()
    {
        setPcDic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Tt is : " + getResult("test", 0.25)); 
        }
    }

    public void setPcDic()
    {
        P_CDic.Add(0.05, 0.0038);
        P_CDic.Add(0.1, 0.015);
        P_CDic.Add(0.15, 0.032);
        P_CDic.Add(0.20, 0.056);
        P_CDic.Add(0.25, 0.085);
        P_CDic.Add(0.30, 0.12);
        P_CDic.Add(0.35, 0.16);
        P_CDic.Add(0.40, 0.20);
        P_CDic.Add(0.45, 0.25);
        P_CDic.Add(0.50, 0.30);
        P_CDic.Add(0.55, 0.36);
        P_CDic.Add(0.60, 0.42);
        P_CDic.Add(0.65, 0.48);
        P_CDic.Add(0.70, 0.57);
        P_CDic.Add(0.75, 0.67);
        P_CDic.Add(0.80, 0.75);
        P_CDic.Add(0.85, 0.82);
        P_CDic.Add(0.90, 0.89);
        P_CDic.Add(0.95, 0.95);
    }

    public bool getResult(string _name , double _value)
    {
        bool res;
        CValueObj cvobj;

        if (Act_CVObjeDic.ContainsKey(_name))
        {
            // 如果存在
            cvobj = Act_CVObjeDic[_name];
        }
        else
        {
            // 如果不存在
            // 新建且加入
            cvobj= new CValueObj();
            cvobj.setCValueObject(_name, _value);
            Act_CVObjeDic.Add(_name, cvobj);
        }

        // 下面开始计算
        if (P_CDic.ContainsKey(cvobj.PValue))
        {
            double resP = cvobj.cnt * (P_CDic[cvobj.PValue]); //计算结果
            Debug.Log("ResP " + resP);
            double R = Random.Range(0, 1.0f); 

            if(R < resP)
            {
                cvobj.cnt = 1;
                res = true;
            }
            else
            {
                cvobj.cnt++;
                res = false;
            }

        }
        else
        {
            Debug.Log("概率错误： " + cvobj.PValue);
            return false;
        }

        return res; 
    }
}
