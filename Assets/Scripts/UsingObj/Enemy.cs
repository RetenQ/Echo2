using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Chara
{

    // ���ڽ�׷�ٸ�ΪNav
    //public EnemyRoom room;

    public NavMeshAgent2D Nav2dAgent;

    [Header("���˲���")]
    [SerializeField] private bool isActive; //�����Ƿ�����
    [SerializeField] protected GameObject Player;
    [SerializeField] protected PlayerBase PlayerSc;

    public GameObject target;// �����ȵ�Ŀ�꣬һ����Player
    public float AttackCD; // CD����ִ��Attack����
    public float activeArrange; //������Χ - Ŀǰ�Ȳ���
    public float attackArrange; // ������Χ 
    [SerializeField] protected float attackCDTimer;

    [Header("���")]
    public Animator animator;
    public AudioSource audio;  //
    public Rigidbody2D rb;
    public Image hpBar;

    [Header("��Ч")]
    public AudioSource Audio_attack;
    public AudioSource Audio_hurt;

    protected override void ObjAwake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void ObjStart()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
        nowHp = maxHp;

        target = Player;  // Ĭ������Ϊplayer

        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        hpBar = transform.Find("Char_State_UI").Find("HP").gameObject.GetComponent<Image>();

        Nav2dAgent = GetComponent<NavMeshAgent2D>();
        //room.registerEnemy(this);

        attackCDTimer = AttackCD;

    }

    protected override void ObjUpdate()
    {
        DataUpdater();

        FindPlayer();

        if (Vector2.Distance(transform.position, target.transform.position) <= attackArrange)
        {
            // ��ҽ��빥����Χ
            if (attackCDTimer >= 0.01f)
            {
                attackCDTimer -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("?");
                Attack();
                // Debug.Log("!");

                attackCDTimer = AttackCD;
            }
        }

    }

    private void DataUpdater()
    {
        hpBar.fillAmount = nowHp * 1.0f / maxHp;

    }

    public override void Hurt(float _damage, BaseObj _hurtby)
    {
        // Debug.Log("?"); 
        nowHp -= _damage;
        Audio_hurt.Play();
        animator.Play("Hurt");

        _hurtby.UpdateLastAttack(this);
        lastHurtby = _hurtby;
    }

    public virtual void FindPlayer()
    {
        // Ѱ��player
        Nav2dAgent.destination = PlayerSc.transform.position;
    }
    public virtual void Attack()
    {
        Debug.Log("���˹���");
        // 
    }




    public float radius = 5f;

    // Բ�ķֶ�����Խ��Խƽ���������ڱ༭���е���
    public int segments = 50;

    // Gizmos����ɫ�������ڱ༭���е���
    public Color color = Color.white;

    // �ڳ�����ͼ�л���Gizmos
    private void OnDrawGizmos()
    {
        radius = attackArrange;
        color = Color.yellow;
        // ����Gizmos����ɫ
        Gizmos.color = color;

        // ��ȡ�����λ��
        Vector3 position = transform.position;

        // ����ÿ���ֶεĽǶȣ���λΪ����
        float angle = 2 * Mathf.PI / segments;

        // ����ÿ���ֶ�
        for (int i = 0; i < segments; i++)
        {
            // ���㵱ǰ�ֶε�������꣬���������λ��
            float x1 = radius * Mathf.Cos(i * angle);
            float y1 = radius * Mathf.Sin(i * angle);
            Vector3 point1 = new Vector3(x1, y1, position.z) + position;

            // ������һ���ֶε�������꣬���������λ��
            float x2 = radius * Mathf.Cos((i + 1) * angle);
            float y2 = radius * Mathf.Sin((i + 1) * angle);
            Vector3 point2 = new Vector3(x2, y2, position.z) + position;

            // ����������֮����߶Σ��γ�Բ��
            Gizmos.DrawLine(point1, point2);
        }
    }
}
