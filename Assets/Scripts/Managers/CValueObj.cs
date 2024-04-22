using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CValueObj : MonoBehaviour
{
    public string ActionName; //行动名称
    public double PValue; //概率
    public int cnt; //使用次数

    public void setCValueObject(string _name , double _value)
    {
        this.ActionName= _name;
        this.PValue= _value;
        this.cnt = 1;
    }
}
