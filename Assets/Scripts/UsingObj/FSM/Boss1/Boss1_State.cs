using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ֹһ���࣬��Ҫ�õ���ת��״̬���඼д��������    
public class Boss1_State : IState
{
    // Idle
    private Boss1 manager;

    public Boss1_State(Boss1 _manager)
    {
        this.manager = _manager;
        //���췽�����󶨶�Ӧ��FSM
    }
    public void OnEnter()
    {

    }
    public void OnUpdate()
    {
     

    }
    public void OnExit()
    {

    }

}

public class ChaseState : IState
{
    private Boss1 manager;
    private Enemy enemy;
    private float maxTime = 2.0f; 
    public ChaseState(Boss1 _manager)
    {
        this.manager = _manager;
    }
    public void OnEnter()
    {
        manager.Nav2dAgent.destination = manager.target.transform.position;
        manager.RandomAttack();

    }
    public void OnUpdate()
    {
        manager.Nav2dAgent.destination = manager.target.transform.position;
        if(Vector2.Distance(manager.transform.position , manager.target.transform.position) <= 0.5f)
        {
            manager.TransitionState(StateType.Idle); 
        }

        // ʱ��ľ�Ҳ�˳�
        if(maxTime <= 0.01f)
        {
            manager.TransitionState(StateType.Idle);

        }
        else
        {
            maxTime -= Time.deltaTime; 
        }

    }
    public void OnExit()
    {
        manager.RandomAttack();
        manager.transform.position = manager.MovePoint1.transform.position; //��λ��1��λ
    }

}

public class LaserSate : IState
{
    private Boss1 manager;

    private List<Laser> lasers;

    private float timer = 1.0f; 
    private int maxCnt = 10; 

    public LaserSate(Boss1 _manager)
    {
        this.manager = _manager;
        //���췽�����󶨶�Ӧ��FSM
    }
    public void OnEnter()
    {
        lasers = new List<Laser>();
        createLaser();
    }
    public void OnUpdate()
    {
        if(maxCnt> 0)
        {
            if (timer <= 0)
            {
                createLaser();
                timer = 0.2f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            manager.TransitionState(StateType.Idle); 
        }

    }
    public void OnExit()
    {
        foreach(Laser l in lasers)
        {
            l.ActiveLaser();
            
        }

        Debug.Log(maxCnt);
        maxCnt = 10;

        lasers.Clear();

        manager.RandomAttack();

    }

    private void createLaser()
    {
        GameObject tmpL = GameObject.Instantiate(manager.LaserObj, manager.transform.position, Quaternion.identity);
        Laser _laser = tmpL.GetComponent<Laser>();
        _laser.setLaser(manager.target, "Player", manager.attack, manager);
        _laser.putLaser();
        lasers.Add(_laser);
        maxCnt--;

    }

}


public class SoulBox_State : IState
{
    private Boss1 manager;
    private Enemy enemy;

    public SoulBox_State(Boss1 _manager)
    {
        this.manager = _manager;
        //���췽�����󶨶�Ӧ��FSM
    }
    public void OnEnter()
    {

    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        // �˳�ʱ�Զ�����һ��
        manager.RandomAttack();
    }

}

public class Move_State : IState
{
    private Boss1 manager;
    private Enemy enemy;
    private GameObject des; 

    public Move_State(Boss1 _manager)
    {
        this.manager = _manager;
        //���췽�����󶨶�Ӧ��FSM
    }
    public void OnEnter()
    {
        int tmp = Random.Range(1, 4); 
        if(tmp == 1)
        {
            des = manager.MovePoint1; 
        }else if( tmp == 2)
        {
            des = manager.MovePoint2;

        }
        else
        {
            des = manager.MovePoint3;

        }
    }
    public void OnUpdate()
    {
       manager.Nav2dAgent.destination = des.transform.position; // �ƶ�
       manager.PayloadCnt= 0;
       if(Vector2.Distance(manager.transform.position , des.transform.position) < 0.01f){
            manager.TransitionState(StateType.Laser);
        }

    }
    public void OnExit()
    {
        manager.RandomAttack();
    }

}


//! Dead����һ����Enemy�ű�������

public class DeadState : IState
{
    private Boss1 manager;
    private Enemy enemy;

    public DeadState(Boss1 _manager)
    {
        this.manager = _manager;
        //���췽�����󶨶�Ӧ��FSM
    }
    public void OnEnter()
    {

    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {

    }

}

