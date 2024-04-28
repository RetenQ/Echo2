using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

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
    public float AllowPressAdvance; //������ǰ��� �� ʵ�ʺ�delayPlaySET�����������ӳٴ�����ʱ��

    public float startPlay; // ����ʱ���֮��ʼ����ϵͳ
    public float delayPlaySET; // �ӳٲ��ţ����ã�
    [SerializeField]private float delayPlayTimer; // �ӳٲ���

    public bool isRhy = false;
    [SerializeField] private float timeToArrive; // ���ڼ���UI
    [SerializeField] private float delayPlay_Record;



    [Header("ע����")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;
    public List<BaseObj> Objs;

    [Header("UI����")]
    public GameObject canvasObj; // UI Canvas������  
    public RectTransform canvas; // UI Canvas������  
    public Sprite imageSprite; // Ҫʹ�õ�Image��Sprite  
    public Vector2 spawnPosition; // Image������λ��  
    public float moveSpeed = 100f; // Image���ƶ��ٶ�  
    public float UIScale; 

    private Image movingImage; // ��̬������Image  
    private float disappearanceTime = 1f; // Image��ʧ��ʱ�䣨�룩  

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


        delayPlayTimer = delayPlaySET;
        delayPlay_Record = delayPlayTimer;

        timeToArrive = delayPlay_Record;

        // kore
        Koreographer.Instance.RegisterForEvents(eventID, DrumBeat); // ע��


        canvasObj = GameObject.Find("MainUI_Playing").gameObject;
        canvas = canvasObj.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        // ����
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

        // �ӳٲ�������
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
        Debug.Log("���ܵ��� " + Time.time);
        genRhyUI(); //����UI

        if (koreographyEvent.HasIntPayload())
        {
            int tmp = koreographyEvent.GetIntValue();

            // ����ǲ����˳��Ĵ��룬��99
            if(tmp == 99)
            {
                // ��Ϸ����
                GameManager.GetInstance().GameOver();
            }

            StartCoroutine(DrumBeatDealay(true , tmp ));
        }
        else
        {
            StartCoroutine(DrumBeatDealay(false, 0));

        }


        //genRhyUI();
        // ���ĵ��˸�ʲô    
        // ����payloads
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

        yield return new WaitForSeconds(delayPlaySET - AllowPressAdvance); //ʵ���������˿�����ǰ��ô���

        Debug.Log("Delayed function executed after " + delayPlaySET + " seconds");

        DrumBeatDealay_Real(havePayLoad , tmp);

    }

    private void DrumBeatDealay_Real(bool havePayLoad, int tmp)
    {
        Debug.Log("���õ��� " + Time.time);

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

    public void genRhyUI()
    {
        Debug.Log("����Drum "+Time.time);

        // �����µ�Image��������λ�ú�Sprite  
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
        // ʹImageһֱ�����ƶ�  
        while (true)
        {
            img.rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

            // ����Ƿ�������Image��ʱ��  
            if (delay <= 0)
            {
                Destroy(img.gameObject);
                break;
            }

            // ����ʣ��ʱ��  
            delay -= Time.deltaTime;

            yield return null;
        }
    }

}
