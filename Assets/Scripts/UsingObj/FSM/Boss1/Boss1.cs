using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StateType
{
    //枚举所有的状态
    Idle,Patrol, Attack, Dead,Drum
}

public class Boss1 : FSM_Enemy
{
/*    [SerializeField] protected IState currentState;
    protected Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();*/

    protected override void ObjAwake()
    {
        // 初始化状态机
        states.Add(StateType.Idle , new Boss1_State(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Dead, new DeadState(this));
        states.Add(StateType.Drum, new DrumState(this));

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

    //     public void TransitionState(StateType type)

}
