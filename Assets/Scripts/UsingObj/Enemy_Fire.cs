using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fire : Enemy
{
    public override void Attack()
    {
        Audio_attack.Play();
        GameObject bullet_temp = Instantiate(Enemybullet, firePoint.transform.position, Quaternion.identity);
        bullet_temp.GetComponent<Bullet>().SetBullet( attack , this); //���ӵ��˺�����Ϊ��ɫ������

        // ���㵽��ҵķ���
        Vector2 direction = (Player.transform.position - firePoint.transform.position).normalized;
       // Debug.Log("Dir" + direction);
        bullet_temp.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }


}
