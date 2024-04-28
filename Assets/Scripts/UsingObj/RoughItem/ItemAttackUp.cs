using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttackUp : RogueItem
{

    public override void ItemFun()
    {
        base.ItemFun();


        Itemuser.attack += 5;
    }
}
