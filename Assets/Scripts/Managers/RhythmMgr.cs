using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 管理BGM的东西
public class RhythmMgr : SingletonMono<RhythmMgr>
{
    [Header("BGM设置区")]
    [SerializeField] private bool isRealWorking = true; // 设置为true才会进行工作。如果被StopAllMusicRhy设置为false则停工

    public GameObject realPlayer;
    public AudioSource realAudio;

    [SerializeField] private bool isActive = false;
    public string eventID;
    public float RhyTolerance; // isRhy为True多久之后改回false
    public float RhyToleranceTimer;
    public float startPlay; // 倒计时多久之后开始节奏系统
    public float delayPlay; // 延迟播放，实际上是可以提前多少秒踩点

    public bool isRhy = false;
    [SerializeField] private float timeToArrive; // 用于计算UI
    [SerializeField] private float delayPlay_Record;



    [Header("注册区")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;
    public List<BaseObj> Objs;

    [Header("组件")]
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
        realAudio = realPlayer.GetComponent<AudioSource>(); // 真实的播放器
        realAudio.clip = audioSource.clip;

        delayPlay_Record = delayPlay;

        timeToArrive = delayPlay_Record;

        // kore
        Koreographer.Instance.RegisterForEvents(eventID, DrumBeat); // 注册

    }

    // Update is called once per frame
    void Update()
    {
        // 测试
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

        // 延迟播放设置
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
        // 到鼓点了干什么    
        // 测试payloads
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
        // 播放音乐谱。二者之间的延迟实际上就是delayPlay
        realAudio.Play();
    }

    private void RhyMgrStart()
    {
        // 播放节奏谱，也就是压点的谱
        isActive = true;
        audioSource.Play();
    }

    // 控制Obj
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


        isRhy = true; //设置为True
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

        // 停用所有的，由Rhy控制的Player

        isRealWorking = false;

        realAudio.Stop();
        audioSource.Stop();

    }

}
