using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *    
 *    
    protected override void ObjAwake()
    {

    }

    protected override void ObjStart()
    {

    }

    protected override void ObjUpdate()
    {
        
    }
*/

public class BaseObj : MonoBehaviour
{
    [Header("������ֵ")]
    public bool alive = true ; 

    public float maxHp;
    public float nowHp;

    public float attack; // ���������������ֱ�ӱ�ʾ�ɹ����л��ж����˺� 
    public float speed;

    [Header("���ݼ�¼��")]
    public BaseObj lastAttackto; //�ϴι����Ķ���
    public BaseObj lastHurtby; //�ϴδ���Ķ���


    [Header("������Ӧ����")]
    public bool isRhyObj = false; //�Ƿ��ǿ�����Ӧ���������
    public bool isRhyAct = false;
    public int KorePayload; 

    private void Awake()
    {
        nowHp = maxHp;  // ��ʼ������Ϊ���ֵ
        ObjAwake();


    }

    protected virtual void ObjAwake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("dwdawdawdawd + " + gameObject.name + isRhyObj);

        if (isRhyObj)
        {
            Debug.Log("dwdaw + "+ gameObject.name);
            // ������ڽ���ϵͳ�е����壬��Ҫע��
            RhythmMgr.GetInstance().RegistertObj(this);
        }
        ObjStart();
    }

    protected virtual void ObjStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(nowHp <= 0)
        {
            Death();
        }

        if(isRhyObj)
        {
            if(isRhyAct)
            {
                RhyAction(); 
                isRhyAct=false;
            }
        }

        ObjUpdate();
    }

    protected virtual void ObjUpdate()
    {
        // ÿһ��Obj�Լ���Update

    }

    public void Death()
    {
        if (alive)
        {
            // �����ŲŴ���

            if (isRhyObj)
            {
                RhythmMgr.GetInstance().RemoveObj(this); // ���ע��
            }

            KillNotify(lastHurtby);

            ObjDeath();

            alive = false;
        }

    }

    protected void KillNotify(BaseObj _obj)
    {

        // Debug.Log(gameObject.name +" Killed by  " + _obj.gameObject.name);
        // ֪ͨ��ɱ��
        _obj.KillNotif_Recive(this);
    }

    protected void KillNotif_Recive(BaseObj _obj)
    {
        // ����֪ͨ
        //Debug.Log(gameObject.name + " kill : " + _obj.gameObject.name);
    }

    public virtual void ObjDeath()
    {
        // ����������������ʱ��Ķ������
    }

    public virtual void Hurt(float _damage , BaseObj _hurtby)
    {
        if(gameObject.CompareTag("Wall"))
        {
            // Ŀǰ����ǽ���˺�
        }
        else
        {
            
            nowHp -= _damage;
        }

        _hurtby.UpdateLastAttack(this);
        lastHurtby = _hurtby;
    }


    public void UpdateLastAttack( BaseObj _obj)
    {
        this.lastAttackto = _obj;
    }

    public void UpdateLastHurt(BaseObj _obj)
    {
        this.lastHurtby = _obj;
    }

    public virtual void Heal(float _heal)
    {
        if(nowHp + _heal <= maxHp)
        {
            nowHp += _heal;
        }

        if(nowHp > maxHp)
        {
            nowHp = maxHp;
        }
    }

    public void RhyActOn(int num)
    {
        // ��Rhy�����򿪣������ͷ�ʱ������Update�н���
        isRhyAct = true;
        KorePayload = num; 
    }

    public virtual void RhyAction()
    {
        // ��Ӧ����ϵͳ�ľ������
    }
    
}
