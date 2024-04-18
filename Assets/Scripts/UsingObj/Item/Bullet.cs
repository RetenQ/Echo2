using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private Rigidbody2D rb;

    // �ӵ�
    public BaseObj shooter; //����ӵ��Ķ���
    public float damage;
    public float damageMul = 1.0f;
    public float maxLifeTime;
    public string targetStr;
    public string ignoreStr;

    [SerializeField] private bool isActive = true; 
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
    }
    public void SetBullet(  float _damage , BaseObj _shooter)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        //ע���ӵ������󲻻�ݻ�
        if(isActive)
        {
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



            // ��֤�ӵ�ֻ�ᱻ����һ��
            isActive = false;
        }

    }
}
