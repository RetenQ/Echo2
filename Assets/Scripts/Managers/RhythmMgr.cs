using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

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
    public float AllowPressAdvance; //允许提前多久 ， 实际和delayPlaySET配合算出可以延迟触发的时间

    public float startPlay; // 倒计时多久之后开始节奏系统
    public float delayPlaySET; // 延迟播放（设置）
    [SerializeField]private float delayPlayTimer; // 延迟播放

    public bool isRhy = false;
    [SerializeField] private float timeToArrive; // 用于计算UI
    [SerializeField] private float delayPlay_Record;



    [Header("注册区")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;
    public List<BaseObj> Objs;

    [Header("UI区域")]
    public GameObject canvasObj; // UI Canvas的引用  
    public RectTransform canvas; // UI Canvas的引用  
    public Sprite imageSprite; // 要使用的Image的Sprite  
    public Vector2 spawnPosition; // Image的生成位置  
    public float moveSpeed = 100f; // Image的移动速度  
    public float UIScale; 

    private Image movingImage; // 动态创建的Image  
    private float disappearanceTime = 1f; // Image消失的时间（秒）  

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


        delayPlayTimer = delayPlaySET;
        delayPlay_Record = delayPlayTimer;

        timeToArrive = delayPlay_Record;

        // kore
        Koreographer.Instance.RegisterForEvents(eventID, DrumBeat); // 注册


        canvasObj = GameObject.Find("MainUI_Playing").gameObject;
        canvas = canvasObj.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        // 测试
        if(Input.GetKeyDown(KeyCode.N)) genRhyUI();
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
                if(delayPlayTimer <=0)
                {
                    PlayRealAudio();
                }
                else
                {
                    delayPlayTimer -= Time.fixedDeltaTime;   
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
        Debug.Log("接受到： " + Time.time);
        genRhyUI(); //生成UI

        if (koreographyEvent.HasIntPayload())
        {
            int tmp = koreographyEvent.GetIntValue();

            // 检测是不是退出的代码，即99
            if(tmp == 99)
            {
                // 游戏结束
                GameManager.GetInstance().GameOver();
            }

            StartCoroutine(DrumBeatDealay(true , tmp ));
        }
        else
        {
            StartCoroutine(DrumBeatDealay(false, 0));

        }


        //genRhyUI();
        // 到鼓点了干什么    
        // 测试payloads
/*        if (koreographyEvent.HasIntPayload())
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
        }*/
        //
    }

    IEnumerator DrumBeatDealay(bool havePayLoad , int tmp)
    {

        Debug.Log("Coroutine started, waiting for " + delayPlaySET + " seconds...");

        yield return new WaitForSeconds(delayPlaySET - AllowPressAdvance); //实际上设置了可以提前多久触发

        Debug.Log("Delayed function executed after " + delayPlaySET + " seconds");

        DrumBeatDealay_Real(havePayLoad , tmp);

    }

    private void DrumBeatDealay_Real(bool havePayLoad, int tmp)
    {
        Debug.Log("调用到： " + Time.time);

        if (havePayLoad)
        {
            NotifyObjs(tmp);
            PlayerRhyOn();
        }
        else
        {
            NotifyObjs(0);
            PlayerRhyOn();
        }
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

    public void genRhyUI()
    {
        Debug.Log("生成Drum "+Time.time);

        // 创建新的Image并设置其位置和Sprite  
        GameObject newImageObj = new GameObject("Moving Image");
        movingImage = newImageObj.AddComponent<Image>();
        movingImage.rectTransform.localScale = new Vector3(UIScale, UIScale, 1.0f); 
        movingImage.sprite = imageSprite;
        movingImage.rectTransform.SetParent(canvas, false);
        movingImage.rectTransform.anchoredPosition = spawnPosition;

        StartCoroutine(MoveAndDestroy(movingImage, disappearanceTime));

    }

    IEnumerator MoveAndDestroy(Image img, float delay)
    {
        // 使Image一直向左移动  
        while (true)
        {
            img.rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

            // 检查是否到了销毁Image的时间  
            if (delay <= 0)
            {
                Destroy(img.gameObject);
                break;
            }

            // 减少剩余时间  
            delay -= Time.deltaTime;

            yield return null;
        }
    }

}
