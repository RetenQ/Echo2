using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedUp_DashOff : RogueItem
{
    [Header("������")]
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

        // �������д����Ҫ�ӳ�ִ�еĴ���

        Itemuser.speed -= addSpeed;

    }

}
