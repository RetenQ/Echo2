using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedUp_DashOff : RogueItem
{
    [Header("设置区")]
    public float addSpeed;
    public float recTime; 

    public override void ItemFun()
    {
        base.ItemFun();


        Itemuser.speed += addSpeed;

        GameManager.GetInstance().DelayedFunction(recTime, speedRecovery);


    }

    private void speedRecovery()
    {

        // 在这里编写你想要延迟执行的代码

        Itemuser.speed -= addSpeed;

    }

}
