using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
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
    public GameObject firePoint; //���λ��
    public GameObject Enemybullet;
    public float bulletSpeed;



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

    [Header("ͨ�ù����ű�")]
    [Header("ɢ�䵯Ļ")]
    public float rotationAngle_Set;
    public float rotationChange;
    public int bulletNum;
    public int bulletWave; //����
    public GameObject MulfirePoint;
    public GameObject MulBullet;
    [Header("360�ȵ�Ļ")]
    public GameObject FullfirePoint;
    public GameObject FullBullet;
    public int FullBulletNum;
    public int FullBulletWave;
    [Header("�����")]
    public GameObject RocketBullet;
    public int RocketBulletNum;
    public int RocketBulletWave;
    public float rocketBulletLifeTime;
    public float rocketAngle;
    public float rocketLerp;

    [Header("����")]
    private bool nothing; // ��Ϊ�ָ��ı���



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
        firePoint = transform.Find("Firepoint").gameObject; 

        Nav2dAgent = GetComponent<NavMeshAgent2D>();
        //room.registerEnemy(this);

        attackCDTimer = AttackCD;

        // ͨ��������
        if(MulBullet == null && Enemybullet != null)
        {
            MulBullet = Enemybullet; 
        }

        if(FullBullet == null && Enemybullet != null)
        {
            FullBullet = Enemybullet;
        }

        if (RocketBullet == null && Enemybullet != null)
        {
            RocketBullet = Enemybullet;
        }

        

        MulfirePoint = firePoint;
        FullfirePoint = gameObject ;  // 360ɢ��������Ϊ����

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


        // ����
        if (Input.GetKeyDown(KeyCode.U))
        {
            MulAttack();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            FullAttack();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            RocketAttack();
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


    // ����ΪһЩ���˵�ͨ�ù�������
    public virtual void MulAttack()
    {
        //��Ļ������Ĭ���Ƿ���5öһ���ɢ��
        StartCoroutine(MulBulletAttack());

    }

    IEnumerator MulBulletAttack()
    {
        // Debug.Log("����");
        // Vector3 _position = (target.transform.position - firePoint.transform.position); 
        // ����Ƕ�
        Vector3 _position = MulfirePoint.transform.position;
        rotationAngle_Set = 360 - (CalRotation(MulfirePoint.transform.position, target.transform.position));
        float rotationAngle = rotationAngle_Set;
        // Debug.Log("ITS"+rotationAngle);

        // �ӵ�����
        for (int k = 0; k < bulletWave; k++)
        {

            int half = bulletNum / 2; //ÿ������
            CreateBullet(rotationAngle, _position, MulBullet);//����������
            for (int j = 0; j < half; j++)
            {
                rotationAngle += rotationChange;
                CreateBullet(rotationAngle, _position, MulBullet);
            }

            rotationAngle = rotationAngle_Set; //��λ

            for (int w = 0; w < half; w++)
            {
                rotationAngle -= rotationChange;
                CreateBullet(rotationAngle, _position, MulBullet);
            }
            rotationAngle = rotationAngle_Set; //��λ
            yield return new WaitForSeconds(0.5f);
        }

    }

    protected float CalRotation(Vector3 a, Vector3 b)
    {
        float x = (a.x - b.x);
        float y = (a.y - b.y);

        float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

        float theCos = x / hypotenuse;
        float radian = Mathf.Acos(theCos);

        float angle = 180 / (Mathf.PI / radian);

        if (a.y >= b.y)
        {
            angle = -angle;
        }

        return angle - 90;

    }

    protected void CreateBullet(float rotationAngle, Vector3 firePoint, GameObject _bullet)
    {
        GameObject _createBullet = Instantiate(_bullet, firePoint, Quaternion.AngleAxis(rotationAngle, Vector3.forward));
        _bullet.GetComponent<Bullet>().SetBulletDirect(this.attack , this.bulletSpeed ,  this);
    }

    public void FullAttack()
    {
        //OK
        StartCoroutine(Fire360Bullet(FullfirePoint.transform.position));
    }

    IEnumerator Fire360Bullet(Vector3 firePosition)
    {
        float rotationAngle = 0f;
        for (int i = 0; i < FullBulletWave; i++)
        {
            for (int j = 0; j < FullBulletNum; j++)
            {
                rotationAngle += (360 / FullBulletNum);
                CreateBullet(rotationAngle, firePosition, FullBullet);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // ������
    public void RocketAttack()
    {
        StartCoroutine(RocketBulletAttack(FullfirePoint.transform.position));
       /* GameObject _createBullet = Instantiate(RocketBullet, firePoint.transform.position, Quaternion.identity);
        _createBullet.GetComponent<Bullet>().SetBulletRocket(this.attack, this.bulletSpeed, target.transform.position, this);*/
    }

    private void CreateRocketBullet(float rotationAngle, Vector3 firePoint, GameObject _bullet)
    {
        GameObject _createBullet = Instantiate(_bullet, firePoint, Quaternion.AngleAxis(rotationAngle, Vector3.forward));
        _bullet.GetComponent<Bullet>().SetBulletRocket(this.attack, this.bulletSpeed -2.0f
            , target.transform.position, rocketLerp , this);
        _bullet.GetComponent<Bullet>().SetBulletLfveTime(rocketBulletLifeTime) ;
    }

    IEnumerator RocketBulletAttack(Vector3 firePosition)
    {
        float rotationAngle = 0f;
        for (int i = 0; i < RocketBulletWave; i++)
        {
            for (int j = 0; j < RocketBulletNum; j++)
            {
                rotationAngle += (360 / RocketBulletNum);
                CreateRocketBullet(rotationAngle, firePosition, RocketBullet);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }


    // ==============================

    // ����Ϊ�༭���з���鿴������Χ����������
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
