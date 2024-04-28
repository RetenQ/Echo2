using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Item_ClearBullet_DashOff : RogueItem
{

    [Header("设置区")]
    public float radius;
    public override void ItemFun()
    {
        base.ItemFun();


        ClearBullet(Itemuser.gameObject.transform.position); 
    }

    public void ClearBullet(Vector2 pos)
    {
        Debug.Log("Clear");

        Collider2D[] colliders = new Collider2D[10]; // 限制数组大小
        int colliderCount = Physics2D.OverlapCircleNonAlloc(pos, radius, colliders);

        for (int i = 0; i < colliderCount; i++)
        {
            Bullet bullet = colliders[i].GetComponent<Bullet>();
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
        }
    }
}
