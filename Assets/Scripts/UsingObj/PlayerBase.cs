using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBase : Chara
{

    [Header("Player属性")]
    [SerializeField] private bool inRhy; // 是否在节奏区间
    [SerializeField] private bool islock = false; //锁定时无法操作
    [SerializeField] private bool isdash = false; 
    [SerializeField] private bool isRun = false; 
                     public bool isAttack = false; 
    private Vector2 movement;
    [SerializeField] private Vector2 lastMovement;//最后一次非0方向

    public Vector2 mouseLocation;
    private Vector2 ToMouseDirection;

    [SerializeField] private int facing;

    public float noHurtTime; //无敌时间
    [SerializeField] public bool isNoHurt = false;

    [Header("冲刺数据")]
    public float dashCD = 2;
    public float dashMul;  // 此处是dash的加速倍数
    public float dashTimer = 0.01f;
    public float maxDashTime = 1.5f;
    public float stopDashTime = 0.1f; //多久可以手动停止
    [SerializeField] private float startDashTimer;
    public GameObject trailEffect; 
    public GameObject trailEffect_ex; 
    public GameObject trailEffect_last;

    [Header("道具部分")]
    public List<GameObject> dataItems = new List<GameObject>(); //type0 读取时就释放
    public List<GameObject> FireItems = new List<GameObject>(); // type1
    public List<GameObject> AttackItems = new List<GameObject>(); // type2
    public List<GameObject> DashOnItems = new List<GameObject>();// type3
    public List<GameObject> DashOffItems = new List<GameObject>();// type4
    public List<GameObject> HurtItems = new List<GameObject>();// type5
    public List<GameObject> RhyonItems = new List<GameObject>(); //状态开始时释放
    public List<GameObject> RhyoffItems = new List<GameObject>();//状态结束时释放

    [Header("节奏区域")]
    public float nowBeatValue; // 目前压点的得分 ， 最高100

    public int levelScore; //每层的得分
    public int attackUpLevel = 0;//攻击力提升次数。目前只能通过beatvalue获得。

    [Header("子弹区")]
    public GameObject bullet;
    public GameObject bullet_ex;
    public Transform firePosition;
    public float bulletSpeed; 
    public float bulletSpeed_ex; 
    public float bulletEx_AttackMul;  // 测试用，设置为2倍

    [Header("组件")]
    public Rigidbody2D rb;
    public Collider2D col;
    public Animator animator;
    public SpriteRenderer sr;

    public AudioClip[] attackClips;
    public AudioClip[] dashClips;

    public AudioSource audio_run;
    public AudioSource audio_attack;
    public AudioSource audio_dash;
    public AudioSource audio_beatValueSkill;

    [Header("子物体")]
    public GameObject UpEffect; 
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

    }

    protected override void ObjStart()
    {
        GameManager.GetInstance().getNewPlayer(gameObject); // 注册
        ReloadtheItems(GameManager.GetInstance().rogueItems_chosen); //读取MGR的内容
        UsingItemsInList(dataItems);

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
                    //非冲刺状态下进行的操作

                    Movement();

                    if (lastMovement.x == -1) facing = 1; //左
                    else if (lastMovement.x == 1) facing = 2; //右
                    else if (lastMovement.y == 1) facing = 3; //上
                    else facing = 4; //下

                    if (facing != 0 && (movement.x != 0 || movement.y != 0))
                    {
                        PlayAnim("run");//运动

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
                        // 如果启动后再次按下冲刺
                        startDashTimer = 0;
                        DashOff();

                    }

                    // 冲刺Mode
                    startDashTimer -= Time.deltaTime;
                    if (startDashTimer <= 0)
                    {
                        // 时间到了结束状态
                        DashOff();

                    }
                    else
                    {

                        // 处于冲刺状态下进行冲刺
                        // 这里位置使用的是最后移动的方向
                        rb.velocity = lastMovement * speed * dashMul;
                    }
                }
            }

            

            if (Input.GetMouseButtonDown(0))
            {
                //左键
                Fire();
            }

            if (Input.GetMouseButtonDown(1))
            {
                //右键
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
            AddBeatPont();

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
        startDashTimer = maxDashTime; // Timer设置为最大冲刺时间倒计时

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

        // 更新movement的参数
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x !=0 || movement.y !=0)
        {
            lastMovement = movement; //记录最后方向
            isRun = true; 
        }else{
            isRun = false;
        }

        //！ 注：GetAxisRaw只返回{-1 ,0 , 1}，不做手柄优化且希望操作干净，故这里使用GetAxisRaw

        // 直接使用velocity更好用，所以这里改了
        //!默认乘50
       // rb.MovePosition(rb.position + movement * speed * Time.deltaTime *50);
        //这里直接使用MovePosition
        //因为是放在Update里面且需要每帧控制，所以使用deltaTime

        rb.velocity = new Vector2(movement.x * speed  , movement.y *speed);


        // anim.SetBool("isRun",true);
        if (movement.x != 0 || movement.y != 0)
        {
            //调整朝向
            //此处默认角色朝向是右侧
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
                
            }

            if (movement.x < 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
            }


            // 左右上下 1234
/*            if (lastMovement.x == -1) return 1; //左
            else if (lastMovement.x == 1) return 2; //右
            else if (lastMovement.y == 1) return 3; //上
            else if (lastMovement.y == -1) return 4; //下
            else return 4;*/

                //return 0;


         }
            else
        {

            // return 0;
        }
    }

    /// <summary>
    /// 停止玩家的动作，目前只是停止位移
    /// </summary>
    public void StopMove()
    {
        rb.velocity = Vector2.zero; // 将速度设置为零向量

    }

    public void StopPlayer()
    {
        // 停止player的所有操作
        StopMove();
        audio_attack.Stop();
        audio_dash.Stop();
        audio_run.Stop();

        islock = true; 
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
        // 需要准确计时的数据放在这里
        if (dashTimer >= 0)
        {
            // isdash = false;
            dashTimer -= Time.deltaTime;
        }

        if(nowBeatValue >= 100)
        {
            ClearBeatValue();//消耗并释放技能
        }
    }


    private void Fire()
    {
        UsingItemsInList(FireItems);

        if (inRhy)
        {
            AddBeatPont();
            audio_attack.clip = attackClips[1];
            audio_attack.Play();

            GameObject bullet_temp = Instantiate(bullet_ex, firePosition.position, Quaternion.identity);
            bullet_temp.GetComponent<Bullet>().SetBullet(attack, bulletEx_AttackMul, this); // 增加伤害
            bullet_temp.GetComponent<Rigidbody2D>().AddForce(ToMouseDirection * bulletSpeed_ex, ForceMode2D.Impulse);
        }
        else
        {
            SubBeatPont();
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
        StopMove(); //攻击时不得移动


        // 播放动画
        isAttack = true;

        PlayAnim("attack"); 
        // 交代给AttackArrange来执行
        // 现在先写普通的攻击模块
        attackAreaSC.setAttackArea(attack , facing , this); //目前设置成和攻击力一样，后面可能考虑加值
        attackArea.SetActive(true);

        // 在attackArea的代码中的Onenable发动攻击和相关内容
    }

    /// <summary>
    /// 真正调用攻击的方法，配合动画的event使用
    /// </summary>
    private void RealAttack()
    {
        attackAreaSC.StartAttack();
    }

    public void PlayerRhyOn()
    {
        //Debug.Log("ON");
        inRhy = true;
        //sr.color = Color.red; 
    }

    public void PlayerRhyOff()
    {
        //Debug.Log("OFF!");

        inRhy = false;
       //sr.color = Color.blue;

    }

    // 控制beat的区域
    // 节奏点击中+10
    public void AddBeatPont()
    {
        if(nowBeatValue < 100)
        {
            nowBeatValue += 10; //默认+10
        }
        levelScore += 3;  //每次成功都给总积分+3;
    }

    public void AddBeatPont(int _value)
    {
        if(nowBeatValue + _value <= 100)
        {
            nowBeatValue += _value;
        }
        else
        {
            nowBeatValue = 1000;
        }
    }

    //  没在节奏点默认-55
    public void SubBeatPont()
    {
        if (nowBeatValue > 5)
        {
            nowBeatValue -= 5; //默认-10
        }
    }

    public void SubBeatPont(int _value)
    {
        if (nowBeatValue - _value >= 0)
        {
            nowBeatValue -= _value;
        }
        else
        {
            nowBeatValue = 0;
        }
    }


    public void ClearBeatValue()
    {
        // 触发一些技能
        audio_beatValueSkill.Play();
        // 目前是是攻击力+10，血量回复30
        this.attack += 10;
        attackUpLevel++;

        GameObject upef = GameObject.Instantiate(UpEffect, transform.position ,Quaternion.identity);

        Heal(30); 
        //
        nowBeatValue = 0;
    }

    public void PlayAnim(string _name)
    {
        string res = _name;

/*        if(_name.Equals("idle"))
        {

        }*/

        // 左右上下 1234
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
                    // 目前不做墙体伤害
                }
                else
                {
                    UsingItemsInList(HurtItems);

                    nowHp -= _damage;

                    NoHurt();
                    // 受击后震动
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

    // ! 测试用，重启游戏
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// 通过List,重新载入
    /// </summary>
    /// <param name="choseItemList"></param>
    public void ReloadtheItems(List<GameObject> choseItemList)
    {
        foreach (GameObject _item in choseItemList)
        {
            RogueItem rogueItem = _item.GetComponent<RogueItem>();
            int tmp = rogueItem.type;
            if (tmp == 0)       dataItems.Add(_item);
            else if (tmp == 1)      FireItems.Add(_item);
            else if (tmp == 2) AttackItems.Add(_item);
            else if (tmp == 3) DashOnItems.Add(_item);
            else if (tmp == 4) DashOffItems.Add(_item);
            else if (tmp == 5) HurtItems.Add(_item);
            else Debug.Log("ADD ITEM FALSE!!!");

            _item.GetComponent<RogueItem>().Itemuser = this; 
        }
        // 首先加入玩家的列表

    }

}
