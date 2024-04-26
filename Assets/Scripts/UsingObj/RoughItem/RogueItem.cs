using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueItem : MonoBehaviour
{
    public string Itemname; //名字
    public string describe; //描述，暂不使用
    public int type; // 类别，插入到哪个部分
    public Sprite itemImg; 

    public BaseObj Itemuser; 
    
    /// <summary>
    /// 绑定使用者
    /// </summary>
    /// <param name="_obj"></param>
    public void addThis(BaseObj _obj)
    {
        Itemuser = _obj;
    }

    /// <summary>
    /// 道具的效果
    /// </summary>
    public virtual void ItemFun()
    {

    }
}
