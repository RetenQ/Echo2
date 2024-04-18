using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player_AttackArea : MonoBehaviour
{
    [Header("������ֵ")]
    [SerializeField] private bool isAttack = false;  // �����ظ�����
    [SerializeField] private float attack;

    [SerializeField] private int playerfacing; // ����

    public float maxAttackTime;
    [SerializeField] private float maxAttackTimer; 

    public float attackRange = 5f;
    private Collider2D[] enemiesInRange;

    
    public PlayerBase playerSC;

    private void Awake()
    {
        playerSC = GameObject.FindWithTag("Player").GetComponent<PlayerBase>();
    }

    void FixedUpdate()
    {
        if(maxAttackTimer <= 0.0f)
        {
            // �ָ�
            maxAttackTimer = maxAttackTime;
            StopAttack();
        }
        else
        {
            maxAttackTimer -= Time.fixedDeltaTime; 
        }
    }

    // PlayerBase���͹���->PlayerBase�޸�isAttack = false ->
    // StartAttackִ�й��� , ִ��֮���޸�isAttack = true-> һ������ʱִ��StopAttack
    // -> StopAttackִ�ж�Ӧ����

    private void OnEnable()
    {
        //  StartAttack();
        // ��Ϊ��Player�Ķ����Ĺؼ�֡���
    }

    public void StartAttack()
    {
        if(isAttack == false)
        {
            // ������PlayerBase���ţ���

            // ִ�в��� 
            // ���ֶ�������Trigger��Χ���
            // ��ⷶΧ�ڵ����е���
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D enemyCollider in enemiesInRange)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    // ��¼�ڹ�����Χ�ڵĵ���
                    //Debug.Log("Enemy detected: " + enemyCollider.gameObject.name);
                    enemyCollider.GetComponent<Enemy>().Hurt(attack, playerSC);

                }
            }
            
            isAttack = true;
        }

    }

    private void StopAttack()
    {
        gameObject.SetActive(false); // �ص�
        playerSC.isAttack = false; 
    }

    public void setAttackArea(float _attack , int _facing , PlayerBase _player)
    {
        this.playerfacing = _facing;
        this.attack = _attack; // ΪPlayerBase�õ�
        this.isAttack = false; // �Զ����û�û����

        this.playerSC = _player;       
    }

    void OnDrawGizmos()
    {
        OnDrawGizmosSelected();
    }

    void OnDrawGizmosSelected()
    {
        // �ڱ༭���л��ƹ�����Χ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
