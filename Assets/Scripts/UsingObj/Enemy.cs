using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Chara
{

    // 后期将追踪改为Nav
    //public EnemyRoom room;

    public NavMeshAgent2D Nav2dAgent;

    [Header("敌人参数")]
    [SerializeField] private bool isActive; //敌人是否启动
    [SerializeField] protected GameObject Player;
    [SerializeField] protected PlayerBase PlayerSc;
    public GameObject firePoint; //射击位置
    public GameObject Enemybullet;
    public float bulletSpeed;



    public GameObject target;// 攻击等的目标，一般是Player
    public float AttackCD; // CD到了执行Attack参数
    public float activeArrange; //启动范围 - 目前先不用
    public float attackArrange; // 攻击范围 
    [SerializeField] protected float attackCDTimer;

    [Header("组件")]
    public Animator animator;
    public AudioSource audio;  //
    public Rigidbody2D rb;
    public Image hpBar;

    [Header("音效")]
    public AudioSource Audio_attack;
    public AudioSource Audio_hurt;

    [Header("通用攻击脚本")]
    [Header("散射弹幕")]
    public float rotationAngle_Set;
    public float rotationChange;
    public int bulletNum;
    public int bulletWave; //波数
    public GameObject MulfirePoint;
    public GameObject MulBullet;
    [Header("360度弹幕")]
    public GameObject FullfirePoint;
    public GameObject FullBullet;
    public int FullBulletNum;
    public int FullBulletWave;
    [Header("火箭弹")]
    public GameObject RocketBullet;
    public int RocketBulletNum;
    public int RocketBulletWave;
    public float rocketBulletLifeTime;
    public float rocketAngle;
    public float rocketLerp;

    [Header("辅助")]
    private bool nothing; // 作为分隔的变量



    protected override void ObjAwake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void ObjStart()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
        nowHp = maxHp;

        target = Player;  // 默认设置为player

        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        hpBar = transform.Find("Char_State_UI").Find("HP").gameObject.GetComponent<Image>();
        firePoint = transform.Find("Firepoint").gameObject; 

        Nav2dAgent = GetComponent<NavMeshAgent2D>();
        //room.registerEnemy(this);

        attackCDTimer = AttackCD;

        // 通用项设置
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
        FullfirePoint = gameObject ;  // 360散射以自身为中心

    }

    protected override void ObjUpdate()
    {
        DataUpdater();

        FindPlayer();

        if (Vector2.Distance(transform.position, target.transform.position) <= attackArrange)
        {
            // 玩家进入攻击范围
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


        // 测试
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
        // 寻找player
        Nav2dAgent.destination = PlayerSc.transform.position;
    }
    public virtual void Attack()
    {
        Debug.Log("敌人攻击");
        // 
    }


    // 下面为一些敌人的通用攻击参数
    public virtual void MulAttack()
    {
        //弹幕攻击，默认是发射5枚一组的散弹
        StartCoroutine(MulBulletAttack());

    }

    IEnumerator MulBulletAttack()
    {
        // Debug.Log("攻击");
        // Vector3 _position = (target.transform.position - firePoint.transform.position); 
        // 计算角度
        Vector3 _position = MulfirePoint.transform.position;
        rotationAngle_Set = 360 - (CalRotation(MulfirePoint.transform.position, target.transform.position));
        float rotationAngle = rotationAngle_Set;
        // Debug.Log("ITS"+rotationAngle);

        // 子弹生成
        for (int k = 0; k < bulletWave; k++)
        {

            int half = bulletNum / 2; //每次生产
            CreateBullet(rotationAngle, _position, MulBullet);//先生产中心
            for (int j = 0; j < half; j++)
            {
                rotationAngle += rotationChange;
                CreateBullet(rotationAngle, _position, MulBullet);
            }

            rotationAngle = rotationAngle_Set; //归位

            for (int w = 0; w < half; w++)
            {
                rotationAngle -= rotationChange;
                CreateBullet(rotationAngle, _position, MulBullet);
            }
            rotationAngle = rotationAngle_Set; //归位
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

    // 火箭射击
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

    // 下面为编辑器中方便查看攻击范围的描述部分
    public float radius = 5f;

    // 圆的分段数，越大越平滑，可以在编辑器中调整
    public int segments = 50;

    // Gizmos的颜色，可以在编辑器中调整
    public Color color = Color.white;

    // 在场景视图中绘制Gizmos
    private void OnDrawGizmos()
    {
        radius = attackArrange;
        color = Color.yellow;
        // 设置Gizmos的颜色
        Gizmos.color = color;

        // 获取物体的位置
        Vector3 position = transform.position;

        // 计算每个分段的角度，单位为弧度
        float angle = 2 * Mathf.PI / segments;

        // 遍历每个分段
        for (int i = 0; i < segments; i++)
        {
            // 计算当前分段的起点坐标，相对于物体位置
            float x1 = radius * Mathf.Cos(i * angle);
            float y1 = radius * Mathf.Sin(i * angle);
            Vector3 point1 = new Vector3(x1, y1, position.z) + position;

            // 计算下一个分段的起点坐标，相对于物体位置
            float x2 = radius * Mathf.Cos((i + 1) * angle);
            float y2 = radius * Mathf.Sin((i + 1) * angle);
            Vector3 point2 = new Vector3(x2, y2, position.z) + position;

            // 绘制两个点之间的线段，形成圆弧
            Gizmos.DrawLine(point1, point2);
        }
    }
}
