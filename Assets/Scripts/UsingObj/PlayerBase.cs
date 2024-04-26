using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class PlayerBase : Chara
{

    [Header("Player����")]
    [SerializeField] private bool inRhy; // �Ƿ��ڽ�������
    [SerializeField] private bool islock = false; //����ʱ�޷�����
    [SerializeField] private bool isdash = false; 
    [SerializeField] private bool isRun = false; 
                     public bool isAttack = false; 
    private Vector2 movement;
    [SerializeField] private Vector2 lastMovement;//���һ�η�0����

    public Vector2 mouseLocation;
    private Vector2 ToMouseDirection;

    [SerializeField] private int facing;

    public float noHurtTime; //�޵�ʱ��
    [SerializeField] public bool isNoHurt = false;

    [Header("�������")]
    public float dashCD = 2;
    public float dashMul;  // �˴���dash�ļ��ٱ���
    public float dashTimer = 0.01f;
    public float maxDashTime = 1.5f;
    public float stopDashTime = 0.1f; //��ÿ����ֶ�ֹͣ
    [SerializeField] private float startDashTimer;
    public GameObject trailEffect; 
    public GameObject trailEffect_ex; 
    public GameObject trailEffect_last;

    [Header("���߲���")]
    public List<GameObject> dataItems = new List<GameObject>(); //��ȡʱ���ͷ�
    public List<GameObject> FireItems = new List<GameObject>(); // type1
    public List<GameObject> AttackItems = new List<GameObject>(); // type2
    public List<GameObject> DashOnItems = new List<GameObject>();// type3
    public List<GameObject> DashOffItems = new List<GameObject>();// type4
    public List<GameObject> HurtItems = new List<GameObject>();// type5
    public List<GameObject> RhyonItems = new List<GameObject>(); //״̬��ʼʱ�ͷ�
    public List<GameObject> RhyoffItems = new List<GameObject>();//״̬����ʱ�ͷ�

    [Header("��������")]
    public float nowBeatValue; // Ŀǰѹ��ĵ÷� �� ���100

    public float levelScore; //ÿ��ĵ÷�

    [Header("�ӵ���")]
    public GameObject bullet;
    public GameObject bullet_ex;
    public Transform firePosition;
    public float bulletSpeed; 
    public float bulletSpeed_ex; 

    [Header("���")]
    public Rigidbody2D rb;
    public Collider2D col;
    public Animator animator;
    public SpriteRenderer sr;

    public AudioClip[] attackClips;
    public AudioClip[] dashClips;

    public AudioSource audio_run;
    public AudioSource audio_attack;
    public AudioSource audio_dash;

    [Header("������")]
    public GameObject attackArea; 
    public Player_AttackArea attackAreaSC; 

    protected override void ObjAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        attackArea = transform.Find("AttackArea").gameObject; 
        attackAreaSC = attackArea.GetComponent<Player_AttackArea>();

        UsingItemsInList(dataItems);
    }

    protected override void ObjStart()
    {
        GameManager.GetInstance().getNewPlayer(gameObject); // ע��
        ReloadtheItems(GameManager.GetInstance().rogueItems_chosen); //��ȡMGR������

    }

    protected override void ObjUpdate()
    {

        // Debug.Log("R:" + RightD + " || E:" + ErrorD);

        DataUpdater();

        if (!islock)
        {
            if (!isAttack)
            {
                if (!isdash)
                {
                    //�ǳ��״̬�½��еĲ���

                    Movement();

                    if (lastMovement.x == -1) facing = 1; //��
                    else if (lastMovement.x == 1) facing = 2; //��
                    else if (lastMovement.y == 1) facing = 3; //��
                    else facing = 4; //��

                    if (facing != 0 && (movement.x != 0 || movement.y != 0))
                    {
                        PlayAnim("run");//�˶�

                    }
                    else
                    {
                        PlayAnim("idle");


                    }

                    if (Input.GetKeyDown(KeyCode.LeftControl) && dashTimer <= 0)
                    {
                        DashOn();
                    }
                }
                else
                {
                    PlayAnim("dash");
                    if (Input.GetKeyDown(KeyCode.LeftControl) && startDashTimer <= maxDashTime - stopDashTime)
                    {
                        // ����������ٴΰ��³��
                        startDashTimer = 0;
                        DashOff();

                    }

                    // ���Mode
                    startDashTimer -= Time.deltaTime;
                    if (startDashTimer <= 0)
                    {
                        // ʱ�䵽�˽���״̬
                        DashOff();

                    }
                    else
                    {

                        // ���ڳ��״̬�½��г��
                        // ����λ��ʹ�õ�������ƶ��ķ���
                        rb.velocity = lastMovement * speed * dashMul;
                    }
                }
            }

            

            if (Input.GetMouseButtonDown(0))
            {
                //���
                Fire();
            }

            if (Input.GetMouseButtonDown(1))
            {
                //�Ҽ�
                Attack();
            }
        } 
    }

    private void FixedUpdate()
    {
        FiexdDataUpdater();
    }

    private void UsingItemsInList(List<GameObject> items)
    {
        if(items.Count== 0)
        {
            return; 
        }

        foreach(GameObject _item in items)
        {
            _item.GetComponent<RogueItem>().ItemFun();
            
        }
    }

    private void DashOn()
    {
        UsingItemsInList(DashOnItems);

        if (inRhy)
        {
            trailEffect_ex.SetActive(true);
            trailEffect_last = trailEffect_ex;
            audio_dash.clip = dashClips[1];
            audio_dash.Play();

        }
        else
        {
            trailEffect.SetActive(true);
            trailEffect_last = trailEffect;
            audio_dash.clip = dashClips[0];
            audio_dash.Play();
        }

        isdash = true;
        startDashTimer = maxDashTime; // Timer����Ϊ�����ʱ�䵹��ʱ

        dashTimer = dashCD;
    }

    private void DashOff()
    {

        UsingItemsInList(DashOffItems);
        trailEffect_last.SetActive(false);
        isdash = false;

    }

    public void Movement()
    {

        // ����movement�Ĳ���
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x !=0 || movement.y !=0)
        {
            lastMovement = movement; //��¼�����
            isRun = true; 
        }else{
            isRun = false;
        }

        //�� ע��GetAxisRawֻ����{-1 ,0 , 1}�������ֱ��Ż���ϣ�������ɾ���������ʹ��GetAxisRaw

        // ֱ��ʹ��velocity�����ã������������
        //!Ĭ�ϳ�50
       // rb.MovePosition(rb.position + movement * speed * Time.deltaTime *50);
        //����ֱ��ʹ��MovePosition
        //��Ϊ�Ƿ���Update��������Ҫÿ֡���ƣ�����ʹ��deltaTime

        rb.velocity = new Vector2(movement.x * speed  , movement.y *speed);


        // anim.SetBool("isRun",true);
        if (movement.x != 0 || movement.y != 0)
        {
            //��������
            //�˴�Ĭ�Ͻ�ɫ�������Ҳ�
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
                
            }

            if (movement.x < 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
            }


            // �������� 1234
/*            if (lastMovement.x == -1) return 1; //��
            else if (lastMovement.x == 1) return 2; //��
            else if (lastMovement.y == 1) return 3; //��
            else if (lastMovement.y == -1) return 4; //��
            else return 4;*/

                //return 0;


         }
            else
        {

            // return 0;
        }
    }

    /// <summary>
    /// ֹͣ��ҵĶ�����Ŀǰֻ��ֹͣλ��
    /// </summary>
    public void StopMove()
    {
        rb.velocity = Vector2.zero; // ���ٶ�����Ϊ������

    }

    private void DataUpdater()
    {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ToMouseDirection = (mouseLocation - new Vector2(transform.position.x, transform.position.y)).normalized;

        if (isRun)
        {
            if (!audio_run.isPlaying)
            {
                audio_run.Play();
            }
        }
        else
        {
            audio_run.Pause();
        }
    }

    private void FiexdDataUpdater()
    {
        // ��Ҫ׼ȷ��ʱ�����ݷ�������
        if (dashTimer >= 0)
        {
            // isdash = false;
            dashTimer -= Time.deltaTime;
        }
    }


    private void Fire()
    {
        UsingItemsInList(FireItems);

        if (inRhy)
        {
            audio_attack.clip = attackClips[1];
            audio_attack.Play();

            GameObject bullet_temp = Instantiate(bullet_ex, firePosition.position, Quaternion.identity);
            bullet_temp.GetComponent<Bullet>().SetBullet(attack , 2.0f , this);
            bullet_temp.GetComponent<Rigidbody2D>().AddForce(ToMouseDirection * bulletSpeed_ex, ForceMode2D.Impulse);
        }
        else
        {
            audio_attack.clip = attackClips[0];
            audio_attack.Play();
            GameObject bullet_temp = Instantiate(bullet, firePosition.position, Quaternion.identity);
            bullet_temp.GetComponent<Bullet>().SetBullet(attack,this);
            bullet_temp.GetComponent<Rigidbody2D>().AddForce(ToMouseDirection * bulletSpeed, ForceMode2D.Impulse);
        }

    }

    private void Attack()
    {
        UsingItemsInList(AttackItems);
        StopMove(); //����ʱ�����ƶ�


        // ���Ŷ���
        isAttack = true;

        PlayAnim("attack"); 
        // ������AttackArrange��ִ��
        // ������д��ͨ�Ĺ���ģ��
        attackAreaSC.setAttackArea(attack , facing , this); //Ŀǰ���óɺ͹�����һ����������ܿ��Ǽ�ֵ
        attackArea.SetActive(true);

        // ��attackArea�Ĵ����е�Onenable�����������������
    }

    /// <summary>
    /// �������ù����ķ�������϶�����eventʹ��
    /// </summary>
    private void RealAttack()
    {
        attackAreaSC.StartAttack();
    }

    public void PlayerRhyOn()
    {
        //Debug.Log("ON");
        inRhy = true;
       // sr.color = Color.red; 
    }

    public void PlayerRhyOff()
    {
        //Debug.Log("OFF!");

        inRhy = false;
        //sr.color = Color.blue;

    }

    // ����beat������
    public void AddBeatPont()
    {
        if(nowBeatValue < 100)
        {
            nowBeatValue += 10; //Ĭ��+10
        }
    }

    public void AddBeatPont(float _value)
    {
        if(nowBeatValue + _value <= 100)
        {
            nowBeatValue += _value; 
        }
    }

    public void ClearBeatValue()
    {
        // ����һЩ����

        //
        nowBeatValue = 0;
    }

    public void PlayAnim(string _name)
    {
        string res = _name;

/*        if(_name.Equals("idle"))
        {

        }*/

        // �������� 1234
        if (facing == 3)
        {
            res = "p1-b-" + _name;
        }
        else if (facing == 4)
        {
            res = "p1-f-" + _name;
        }
        else
        {
            res = "p1-s-" + _name;

        }

        //Debug.Log(res);
        animator.Play(res);

    }


    public override void Hurt(float _damage, BaseObj _hurtby)
    {
        if(!isNoHurt)
        {
            if (!isdash)
            {
                if (gameObject.CompareTag("Wall"))
                {
                    // Ŀǰ����ǽ���˺�
                }
                else
                {
                    UsingItemsInList(HurtItems);

                    nowHp -= _damage;

                    NoHurt();
                    // �ܻ�����
                    CameraMgr.GetInstance().ShakeCamera(); 
                }

                _hurtby.UpdateLastAttack(this);
                lastHurtby = _hurtby;
            }
        }


    }

    public void NoHurt()
    {
        isNoHurt = true;
        StartCoroutine(noHurtToNomarl());
    }

    IEnumerator noHurtToNomarl()
    {
        yield return new WaitForSeconds(noHurtTime);
        isNoHurt = false; 

    }

    public override void ObjDeath()
    {
        base.ObjDeath();
        ReloadScene();
    }

    // ! �����ã�������Ϸ
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// ͨ��List,��������
    /// </summary>
    /// <param name="choseItemList"></param>
    public void ReloadtheItems(List<GameObject> choseItemList)
    {
        foreach (GameObject _item in choseItemList)
        {
            RogueItem rogueItem = _item.GetComponent<RogueItem>();
            int tmp = rogueItem.type;

            if (tmp == 1)      FireItems.Add(_item);
            else if (tmp == 2) AttackItems.Add(_item);
            else if (tmp == 3) DashOnItems.Add(_item);
            else if (tmp == 4) DashOffItems.Add(_item);
            else if (tmp == 5) HurtItems.Add(_item);
            else Debug.Log("ADD ITEM FALSE!!!");
        }
        // ���ȼ�����ҵ��б�

    }

}
