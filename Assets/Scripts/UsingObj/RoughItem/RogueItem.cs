using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueItem : MonoBehaviour
{
    public string Itemname; //����
    public string describe; //�������ݲ�ʹ��
    public int type; // ��𣬲��뵽�ĸ�����
    public Sprite itemImg; 

    public BaseObj Itemuser; 
    
    /// <summary>
    /// ��ʹ����
    /// </summary>
    /// <param name="_obj"></param>
    public void addThis(BaseObj _obj)
    {
        Itemuser = _obj;
    }

    /// <summary>
    /// ���ߵ�Ч��
    /// </summary>
    public virtual void ItemFun()
    {

    }
}
