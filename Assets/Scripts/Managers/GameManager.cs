using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    [Header("���")]
    public GameObject player; 
    public PlayerBase playerSC;

    [Header("�÷�")]
    public float theScore; 
    public float theScore_sum; 

    [Header("ȫ�����츳")]
    public List<GameObject> rogueItems_ALL = new List<GameObject>();


    [Header("������ѡȡ�ĵ��ߴ洢")]
    public List<GameObject> rogueItems_canChose = new List<GameObject>();

    [Header("�Ѿ�ѡȡ�ĵ��ߴ洢")]
    public List<GameObject> rogueItems_chosen = new List<GameObject>();

    [Header("�ؿ��б�")]
    public int stage = 1; //����һ��stage��
    public int scene_index; 
    public List<string> scene_list = new List<string>();
    public string mapseed;// ��ͼ����


    [Header("stageA�ĵ�ͼ��")]
    public List<string> stageA_list = new List<string>();

    [Header("safeLevel�ĵ�ͼ��")]
    public List<string> saveLevel_list = new List<string>();

    [Header("���ڴ浵����")]
    public int gameTime; 

    protected override void Awake()
    {
        base.Awake();

        LoadLongSave();

        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        rogueItems_canChose = new List<GameObject>(rogueItems_ALL);  // �ʼ��ʱ���б�����ȫ�������ݵ�
    }

    // Update is called once per frame
    void Update()
    {

        // �����á����ڻ�û��Ŀǰ�����̻���������ȫʹ��SaveSystem (û�кõ�ʱ��ʹ��)
        // ���������������֤���ǿ��е�
        if (Input.GetKeyDown(KeyCode.Keypad0)){
            ShortSaveByJson();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            LoadShortSave();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            LongsaveByJson();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            LoadLongSave();
        }
    }

    public void GenerateMap()
    {
        scene_list.Clear(); // ÿ��ʹ��ǰ��clearһ��
        if (stage == 1)
        {
            GameManager.Shuffle<string>(stageA_list);
            GameManager.Shuffle<string>(saveLevel_list);
            
            // ���� 012 ->��ȫ�� ->3->boss��˳��
            scene_list.Add(stageA_list[0]);
            scene_list.Add(stageA_list[1]);
            scene_list.Add(stageA_list[2]);
            scene_list.Add(saveLevel_list[0]);
            scene_list.Add(stageA_list[3]);
            scene_list.Add("TestBoss"); // ������Ϊ��������һ��Boss��������ֱ�Ӽ��롣���水ÿһ���Boss�����ּ���


        }
    }

    public void generateSeed()
    {
        string tmp = "";
        foreach (string sc in scene_list)
        {
            tmp += sc;// ���� 
        }

        Debug.Log(tmp); //�õ����
        mapseed = tmp; // ��¼


    }

    /// <summary>
    /// ÿһ�ο�ʼ��ʱ��/�л�������ʱ��Player��Ӧ����Mgrע��
    /// </summary>
    /// <param name="_player"></param>
    public void getNewPlayer(GameObject _player)
    {
        player = _player;
        playerSC = player.GetComponent<PlayerBase>(); 
    }


    /// <summary>
    /// ������ҵĵ÷����
    /// </summary>
    public void UpdatePlayerScore()
    {
        theScore= 0; //����

        theScore = playerSC.levelScore;  //����

        theScore_sum += theScore; 
    }

    // ʵ��ϴ���㷨
    // Fisher-Yatesϴ���㷨
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void addItem(GameObject _item)
    {
        RogueItem rogueItem = _item.GetComponent<RogueItem>();

        // ����������ģ��Ѿ�ѡ���б�
        rogueItems_chosen.Add(_item);

        // ���ȼ�����ҵ��б�
        int tmp = rogueItem.type;

        if(tmp== 0) playerSC.dataItems.Add(_item);
        else if (tmp == 1) playerSC.FireItems.Add(_item);
        else if (tmp == 2) playerSC.AttackItems.Add(_item);
        else if (tmp == 3) playerSC.DashOnItems.Add(_item);
        else if (tmp == 4) playerSC.DashOffItems.Add(_item);
        else if (tmp == 5) playerSC.HurtItems.Add(_item);
        else Debug.Log("ADD ITEM FALSE!!!");

        // Ȼ����츳����ɾ��
        rogueItems_canChose.Remove(_item);

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(scene_list[scene_index]); //
        scene_index++; //�����±�
    }

    public void LoadNextScene(string _name)
    {
        SceneManager.LoadScene(_name); //
    }


    // �浵ϵͳ���

    // ShortSave��LongSave����ʽ�ڸò���֮��

    // Shortave
    public ShortSave CreateShortSave()
    {
        // ����һ��save����
        ShortSave save = new ShortSave();

        return save;
    }

    public void ShortSaveByJson()
    {
        //һ���ģ�����ʹ��CreateSaveGameObject()�������õ�Ҫ�����Save
        ShortSave save = CreateShortSave();

        save.theScore_sum = theScore_sum;
        save.rogueItems_canChose = rogueItems_canChose;
        save.rogueItems_chosen = rogueItems_chosen;
        save.stage = stage;
        save.scene_index = scene_index;
        save.mapseed = mapseed;

    //! Json�ı��淽ʽ��JsonString�����������Ҫ����һ����Ӧ��String
    string JsonString = JsonUtility.ToJson(save);
        //���� JsonUtility.ToJson()�õ���������Ҫ�Ķ�����������save��->��JSON�ַ���JsonString��

        //���ʵ����һ����д���ࣺStreamWriter��
        StreamWriter sw = new StreamWriter(Application.dataPath + "ShortSave_DATA.txt");
        //StreamWriter�������ַ����ַ���д���ļ���������д�����ݵĵط��ġ������ļ�·����
        sw.Write(JsonString);//д��
        sw.Close();

        Debug.Log("FINSH: " + Application.dataPath); 
    }

    public void LoadShortSave()
    {
        // ����ļ�����
        if (File.Exists(Application.dataPath + "ShortSave_DATA.txt"))
        {
            //��ȡ�ļ�
            StreamReader sr = new StreamReader(Application.dataPath + "ShortSave_DATA.txt");


            //���ļ�ת��Ϊstring
            string JsonString = sr.ReadToEnd();            
            //ReadToEnd()��ȡ�����������ĵ�ǰλ�õ���β�������ַ�(�������������ĵ�ǰλ�ã���ȡ����β)
            sr.Close();

            // �õ�save�ļ�
            ShortSave save = JsonUtility.FromJson<ShortSave>(JsonString);

            //������ԭ

            theScore_sum = save.theScore_sum;
            rogueItems_canChose = save.rogueItems_canChose;
            rogueItems_chosen = save.rogueItems_chosen;
            stage = save.stage;
            scene_index = save.scene_index;
            mapseed = save.mapseed;
        }
        else
        {
            // �����ڵĻ�����������Ƕ��ڴ浵�����Կ����Ȳ�������
            Debug.Log("����ShortSave");
        }
    }

    // LongSave

    public LongSave CreateLongSave()
    {
        LongSave save = new LongSave();

        return save;
    }

    public void LongsaveByJson()
    {
        //һ���ģ�����ʹ��CreateSaveGameObject()�������õ�Ҫ�����Save
        LongSave save = CreateLongSave();

        save.gameTime= gameTime+1; //�����Ǳ����ʱ��Ĭ�ϼ�һ��

        //! Json�ı��淽ʽ��JsonString�����������Ҫ����һ����Ӧ��String
        string JsonString = JsonUtility.ToJson(save);
        //���� JsonUtility.ToJson()�õ���������Ҫ�Ķ�����������save��->��JSON�ַ���JsonString��

        //���ʵ����һ����д���ࣺStreamWriter��
        StreamWriter sw = new StreamWriter(Application.dataPath + "LongSave_DATA.txt");
        //StreamWriter�������ַ����ַ���д���ļ���������д�����ݵĵط��ġ������ļ�·����
        sw.Write(JsonString);//д��
        sw.Close();
    }

    public void LoadLongSave()
    {
        // ����ļ�����
        if (File.Exists(Application.dataPath + "LongSave_DATA.txt"))
        {
            //��ȡ�ļ�
            StreamReader sr = new StreamReader(Application.dataPath + "LongSave_DATA.txt");


            //���ļ�ת��Ϊstring
            string JsonString = sr.ReadToEnd();
            //ReadToEnd()��ȡ�����������ĵ�ǰλ�õ���β�������ַ�(�������������ĵ�ǰλ�ã���ȡ����β)
            sr.Close();

            // �õ�save�ļ�
            LongSave save = JsonUtility.FromJson<LongSave>(JsonString);

            //������ԭ
            gameTime = save.gameTime;
        }
        else
        {
            // �����ڵĻ��������LongSave������ζ����Ҫ�����³�ʼ������
            gameTime = 1; 
        }
    }


    // �ӳ�ִ��
    public void DelayedFunction(float delay, System.Action function)
    {
        StartCoroutine(DelayedCoroutine(delay, function));
    }

    private IEnumerator DelayedCoroutine(float delay, System.Action function)
    {
        yield return new WaitForSeconds(delay);

        // ������ִ���ӳٺ���
        function?.Invoke();
    }

}


[System.Serializable]
public class LongSave
{
    //������ʾ��ĿǰlongSave��û�кܳ�ֵı������ݡ�����ֻ�洢����Ϸ������Ϊչʾʹ��

    public int gameTime; 
}
