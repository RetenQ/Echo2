using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ֹһ���࣬��Ҫ�õ���ת��״̬���඼д��������    
public class Boss1_State : IState
{
    // idle
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

public class PatrolState : IState
{
    private Boss1 manager;
    private Enemy enemy;
    //ͨ���±���Һ��滻Ѳ�ߵ�
    public PatrolState(Boss1 _manager)
    {
        this.manager = _manager;
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

public class AttackState : IState
{
    private Boss1 manager;

    private AnimatorStateInfo info;

    public AttackState(Boss1 _manager)
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

public class DrumState : IState
{
    private Boss1 manager;
    private Enemy enemy;

    public DrumState(Boss1 _manager)
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