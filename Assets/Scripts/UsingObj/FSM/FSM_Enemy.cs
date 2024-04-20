using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Enemy : Enemy
{
    [SerializeField] protected IState currentState;
    protected Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();



    /// <summary>
    /// �л�״̬����ִ�е�ǰ״̬��exit����һ��״̬��enter
    /// </summary>
    /// <param name="type"></param>
    public void TransitionState(StateType type)
    {
        // �л�״̬

        //Debug.Log("Exit"+currentState);
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = states[type];
        //Debug.Log("Enter"+currentState);
        currentState.OnEnter();
    }

    public float distanceX(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x);
    }
}
