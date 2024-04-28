using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedUp_attack : RogueItem
{
    [Header("设置区")]
    public float addSpeed;
    public float recTime;//多久之后变回来

    public override void ItemFun()
    {

        base.ItemFun();

        Itemuser.speed += addSpeed;

        //Invoke(" speedRecovery", recTime);


        GameManager.GetInstance().DelayedFunction(recTime, speedRecovery); 

    }

    private void speedRecovery()
    {


        Itemuser.speed -= addSpeed;

    }
}
