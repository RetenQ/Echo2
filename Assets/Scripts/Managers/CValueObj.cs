using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CValueObj : MonoBehaviour
{
    public string ActionName; //�ж�����
    public double PValue; //����
    public int cnt; //ʹ�ô���

    public void setCValueObject(string _name , double _value)
    {
        this.ActionName= _name;
        this.PValue= _value;
        this.cnt = 1;
    }
}
