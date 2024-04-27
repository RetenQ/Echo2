using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ����BGM�Ķ���
public class RhythmMgr : SingletonMono<RhythmMgr>
{
    [Header("BGM������")]
    [SerializeField] private bool isRealWorking = true; // ����Ϊtrue�Ż���й����������StopAllMusicRhy����Ϊfalse��ͣ��

    public GameObject realPlayer;
    public AudioSource realAudio;

    [SerializeField] private bool isActive = false;
    public string eventID;
    public float RhyTolerance; // isRhyΪTrue���֮��Ļ�false
    public float RhyToleranceTimer;
    public float startPlay; // ����ʱ���֮��ʼ����ϵͳ
    public float delayPlay; // �ӳٲ��ţ�ʵ�����ǿ�����ǰ������ȵ�

    public bool isRhy = false;
    [SerializeField] private float timeToArrive; // ���ڼ���UI
    [SerializeField] private float delayPlay_Record;



    [Header("ע����")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;
    public List<BaseObj> Objs;

    [Header("���")]
    public AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        Objs = new List<BaseObj>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
        audioSource = GetComponent<AudioSource>();

        realPlayer = transform.Find("realPlayer").gameObject;
        realAudio = realPlayer.GetComponent<AudioSource>(); // ��ʵ�Ĳ�����
        realAudio.clip = audioSource.clip;

        delayPlay_Record = delayPlay;

        timeToArrive = delayPlay_Record;

        // kore
        Koreographer.Instance.RegisterForEvents(eventID, DrumBeat); // ע��

    }

    // Update is called once per frame
    void Update()
    {
        // ����
        if(Input.GetKeyDown(KeyCode.N)) StopAllMusicRhy();
    }

    public float gettimeToArrive()
    {
        return timeToArrive;
    }

    public float getdelayPlay_Record()
    {
        return delayPlay_Record;
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            if(startPlay <=0)
            {
                RhyMgrStart();

            }
            else
            {
                startPlay -= Time.fixedDeltaTime;
            }
        }

        // �ӳٲ�������
        if (!realAudio.isPlaying && isRealWorking)
        {
           if(isActive)
            {
                if(delayPlay <=0)
                {
                    PlayRealAudio();
                }
                else
                {
                    delayPlay -= Time.fixedDeltaTime;   
                }
            }
        }

        if (isRhy)
        {
            if(RhyToleranceTimer >= 0)
            {
                RhyToleranceTimer -= Time.fixedDeltaTime;
            }
            else
            {
                PlayerRhyOff();

            }
        }


        timeToArrive -= Time.fixedDeltaTime;
    }

    private void DrumBeat(KoreographyEvent koreographyEvent)
    {
        // ���ĵ��˸�ʲô    
        // ����payloads
        if (koreographyEvent.HasIntPayload())
        {
            int tmp = koreographyEvent.GetIntValue();
            // Debug.Log("ITS :|||" + tmp);
            NotifyObjs(tmp);
            PlayerRhyOn();
        }
        else
        {
            NotifyObjs(0);
            PlayerRhyOn();
        }
        //
    }

    private void PlayRealAudio()
    {
        // ���������ס�����֮����ӳ�ʵ���Ͼ���delayPlay
        realAudio.Play();
    }

    private void RhyMgrStart()
    {
        // ���Ž����ף�Ҳ����ѹ�����
        isActive = true;
        audioSource.Play();
    }

    // ����Obj
    public void RegistertObj(BaseObj obj)
    {
        Objs.Add(obj);
    }

    public void RemoveObj(BaseObj obj)
    {
        Objs.Remove(obj);
    }
   
    public void NotifyObjs(int value)
    {
        foreach (BaseObj _obj in Objs)
        {
            if(value != 0)
            {
                _obj.RhyActOn(value);

            }
            else
            {
                _obj.RhyActOn(0);
            }
        }
    }

    public void PlayerRhyOn()
    {
        //Debug.Log("ON RHY");

        timeToArrive = delayPlay_Record;


        isRhy = true; //����ΪTrue
        // playerRhy = true;
        PlayerSc.PlayerRhyOn();
    }

    public void PlayerRhyOff()
    {
        //Debug.Log("OFF RHY");

        isRhy = false;
        RhyToleranceTimer = RhyTolerance; 
        // playerRhy = false;
        PlayerSc.PlayerRhyOff();
    }

    public void StopAllMusicRhy()
    {

        // ͣ�����еģ���Rhy���Ƶ�Player

        isRealWorking = false;

        realAudio.Stop();
        audioSource.Stop();

    }

}
