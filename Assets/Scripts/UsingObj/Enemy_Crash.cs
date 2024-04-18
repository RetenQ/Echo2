using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Crash : Enemy
{
    [Header("ײ������")]
    public float dashSpeed = 15f; // ��ײ�ٶ�
    public int attackDamage; //�����˺�
    [SerializeField] private bool hasAttack = false; //�Ѿ������� �� ��ֹ����ظ�����
    public float maxDistance; //����̾���

    public override void Attack()
    {
        // ��ײ�Ĳ�����Ҫ�޸�Nav �� �ݲ���
        hasAttack = false;
        // ���Ŀ���������
        if (target != null)
        {
            //Debug.Log("CrashAttack");
            //Audio_attack.Play();

            // �����ײ�ķ��򣬵�λ����
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // ��ȡ����ĸ������������еĻ�
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // ��������������
            if (rb != null)
            {
                // ������ʩ������ʹ����Ŀ�������ײ
                rb.velocity = direction * dashSpeed; 
            }


        }
    }

    // �������Э��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!hasAttack)
            {
                hasAttack = true;
                PlayerSc.Hurt(attackDamage,this);
                hasAttack = true;

            }

        }
    }
}
