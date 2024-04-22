using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttackUp : RoughItem
{

    public override void ItemFun()
    {
        Itemuser.attack += 5;
    }
}
