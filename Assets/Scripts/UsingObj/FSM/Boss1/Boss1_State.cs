using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//不止一个类，把要用到的转化状态的类都写在这里了    
public class Boss1_State : IState
{
    // idle
    private Boss1 manager;

    public Boss1_State(Boss1 _manager)
    {
        this.manager = _manager;
        //构造方法，绑定对应的FSM
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
    //通过下标查找和替换巡逻点
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
        //构造方法，绑定对应的FSM
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


//! Dead部分一般由Enemy脚本来控制

public class DeadState : IState
{
    private Boss1 manager;
    private Enemy enemy;

    public DeadState(Boss1 _manager)
    {
        this.manager = _manager;
        //构造方法，绑定对应的FSM
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
        //构造方法，绑定对应的FSM
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