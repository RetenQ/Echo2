using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    }

    protected override void ObjUpdate()
    {
        // Update就执行当前的Update
        currentState.OnUpdate();
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

        if(KorePaylaod == 0)
        {
            PayloadCnt++ ;
        }

        if (KorePaylaod == 1 && PayloadCnt >= 3)
        {
            TransitionState(StateType.Chase);
        }else if(KorePaylaod == 2 && PayloadCnt >= 3)
        {
            TransitionState(StateType.Laser);
        }else if(KorePaylaod == 3 && PayloadCnt >= 3)
        {
            TransitionState(StateType.Move);
        }else if(KorePaylaod == 4 && PayloadCnt >= 3)
        {
            TransitionState(StateType.SoulBox);
        }
        else
        {
            RandomAttack();
        }
       
    }

    public void RandomAttack()
    {
        int tmp = Random.Range(1, 4); 

        if(tmp == 1)
        {
            MulAttack();
        }else if(tmp == 2)
        {
            FullAttack();
        }else if(tmp == 3)
        {
            RocketAttack();
        }
        else
        {
            // No
        }
    }

}
