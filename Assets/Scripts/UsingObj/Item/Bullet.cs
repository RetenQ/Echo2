using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private Rigidbody2D rb;

    public bool directlyMove; // ���ø����ʹ�ӵ���һ���Զ������ƶ�����Ƶ�Ļ��ʽ��ʱ����Ҫ���ø���Ϊtrue
    public float directlySpeed; // �����Զ��ƶ����ٶ�

    public bool isRocket;// �Ƿ���׷�ٻ����

    // �ӵ�
    public BaseObj shooter; //����ӵ��Ķ���
    public float damage;
    public float damageMul = 1.0f;
    public float maxLifeTime;
    public string targetStr;
    public string ignoreStr;

    [Header("Rocket����")]
    public float lerp;
    public float rocketSpeed;
    public Vector3 targetPos; 
    public Vector3 rocketDirection;
    [SerializeField] private bool arrived; //�Ƿ��Ѿ���Ŀ��λ���ˣ����˾���ֱ���˶�


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (maxLifeTime >= 0.001f)
        {
            maxLifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        if (directlyMove)
        {
            transform.Translate(Vector3.up * Time.deltaTime * directlySpeed, Space.Self);
        }
    }

    private void FixedUpdate()
    {
        if(isRocket)
        {
            rocketDirection = (targetPos - transform.position).normalized;

            if(!arrived)
            {
                // ����
                transform.right = Vector3.Slerp(transform.right, rocketDirection.normalized , lerp / Vector2.Distance(transform.position , targetPos)) ;

                rb.velocity = transform.right* rocketSpeed;
            }

            if(Vector2.Distance(transform.position , targetPos) < 1f && !arrived)
            {
                arrived= true ;
            }
        }
    }

    public void SetBulletLfveTime(float _time)
    {
        this.maxLifeTime = _time;
    }
    public void SetBullet(  float _damage , BaseObj _shooter)
    {
        this.shooter = _shooter;
        this.damage = _damage;
    }

    public void SetBullet(float _damage, BaseObj _shooter , Vector2 _direct)
    {
        this.shooter = _shooter;
        this.damage = _damage;
    }

    public void SetBullet( float _damage , float _mul , BaseObj _shooter)
    {
        this.shooter = _shooter;

        this.damage = _damage;
        this.damageMul = _mul;
    }

    public void SetBulletDirect(float _damage, float _speed, BaseObj _shooter)
    {
        isRocket = false; 
        this.shooter = _shooter;
        this.damage = _damage;
        this.directlyMove = true;
        this.directlySpeed = _speed;
    }

    public void SetBulletRocket(float _damage , float _speed , Vector2 _target ,float _lerp , BaseObj _shooter)
    {
        directlyMove = false;
        isRocket= true;
        this.lerp = _lerp;
        this.rocketSpeed = _speed;
        this.shooter = _shooter;
        this.damage = _damage;
        targetPos = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " dw  " + collision.name);

        //Debug.Log(collision);
        //ע�������ӵ�����������

            if(collision.CompareTag("Wall"))
            {
                //����ǽ��ֻͣ����
                rb.velocity = Vector3.zero;
                Destroy(gameObject);

            }

            if (collision.CompareTag(targetStr))
            {
                 // Debug.Log("!!!!!!");
                collision.gameObject.GetComponent<BaseObj>().Hurt(damage * damageMul , shooter);  //����˺�

                rb.velocity = Vector3.zero;

                Destroy(gameObject);
            }
            else if (!collision.CompareTag(ignoreStr))
            {
                //Destroy(gameObject);
            }



        

    }
}
