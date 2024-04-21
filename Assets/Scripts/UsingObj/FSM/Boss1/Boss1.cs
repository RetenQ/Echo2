using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum StateType
{
    //枚举所有的状态
    Idle,Chase,Move,Laser,SoulBox,Dead
}

public class Boss1 : FSM_Enemy
{
    /*    [SerializeField] protected IState currentState;
        protected Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();*/

    public int PayloadCnt;
    public GameObject MovePoint1;
    public GameObject MovePoint2;
    public GameObject MovePoint3;

    public GameObject LaserObj;
    protected override void ObjAwake()
    {
        animator = GetComponent<Animator>();


        // 初始化状态机
        states.Add(StateType.Idle , new Boss1_State(this));
        states.Add(StateType.Move, new Move_State(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Laser, new LaserSate(this));
        states.Add(StateType.SoulBox, new SoulBox_State(this));
        states.Add(StateType.Dead, new DeadState(this));

        TransitionState(StateType.Idle); 
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
        if (MulBullet == null && Enemybullet != null)
        {
            MulBullet = Enemybullet;
        }

        if (FullBullet == null && Enemybullet != null)
        {
            FullBullet = Enemybullet;
        }

        if (RocketBullet == null && Enemybullet != null)
        {
            RocketBullet = Enemybullet;
        }



        MulfirePoint = firePoint;
        FullfirePoint = gameObject;  // 360散射以自身为中心
    }

    protected override void ObjUpdate()
    {

        // Update就执行当前的Update
        currentState.OnUpdate();

        if(Input.GetKeyDown(KeyCode.U)) MulAttack();
        if(Input.GetKeyDown(KeyCode.I)) FullAttack();
        if(Input.GetKeyDown(KeyCode.O)) RocketAttack();

    }

    public override void FindPlayer()
    {
        // Boss不自动寻找
        // 寻找player
        // Nav2dAgent.destination = PlayerSc.transform.position;
    }

    //     public void TransitionState(StateType type)

    public override void RhyAction()
    {
        base.RhyAction();
        // 节奏点上的行动

        PayloadCnt++;

        if (KorePayload == 1)
        {
            TransitionState(StateType.Chase);
        }
        else if (KorePayload == 2)
        {
            TransitionState(StateType.Laser);
        }
        else if (KorePayload == 3 )
        {
            TransitionState(StateType.Move);
        }
        else if (KorePayload == 4 )
        {
            TransitionState(StateType.SoulBox);
        }
        else if (KorePayload == 9)
        {
            int tmpState = Random.Range(0, PayloadCnt);
            if (tmpState == 1 )
            {
                TransitionState(StateType.Chase);
            }
            else if (tmpState == 2)
            {
                TransitionState(StateType.Laser);
            }
            else if (tmpState == 3)
            {
                TransitionState(StateType.Move);
            }

        }else if(KorePayload == 5)
        {
            BasicAttack();
        }
        else
        {
            RandomAttack();
        }

    }

    public void BasicAttack()
    {
       // Audio_attack.Play();
        GameObject bullet_temp = Instantiate(Enemybullet, firePoint.transform.position, Quaternion.identity);
        bullet_temp.GetComponent<Bullet>().SetBullet(attack, this); //将子弹伤害设置为角色攻击力

        // 计算到玩家的方向
        Vector2 direction = (Player.transform.position - firePoint.transform.position).normalized;
        // Debug.Log("Dir" + direction);
        bullet_temp.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }

    public void RandomAttack()
    {
        int tmp = Random.Range(1, 4);
        Debug.Log(tmp);

        if (tmp == 1)
        {
            MulAttack();
        }
        else if (tmp == 2)
        {
            FullAttack();
        }
        else if (tmp == 3)
        {
            RocketAttack();
        }
        else
        {
            // No
        }
    }

}
