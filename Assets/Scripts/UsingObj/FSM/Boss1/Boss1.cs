using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StateType
{
    //ö�����е�״̬
    Idle,Patrol, Attack, Dead,Drum
}

public class Boss1 : FSM_Enemy
{
/*    [SerializeField] protected IState currentState;
    protected Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();*/

    protected override void ObjAwake()
    {
        // ��ʼ��״̬��
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
        // Update��ִ�е�ǰ��Update
        currentState.OnUpdate();
    }

    //     public void TransitionState(StateType type)

}
