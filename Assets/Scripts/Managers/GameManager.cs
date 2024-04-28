using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    [Header("玩家")]
    public GameObject player; 
    public PlayerBase playerSC;

    [Header("得分")]
    public float theScore; 
    public float theScore_sum; 

    [Header("全部的天赋")]
    public List<GameObject> rogueItems_ALL = new List<GameObject>();


    [Header("还可以选取的道具存储")]
    public List<GameObject> rogueItems_canChose = new List<GameObject>();

    [Header("已经选取的道具存储")]
    public List<GameObject> rogueItems_chosen = new List<GameObject>();

    [Header("关卡列表")]
    public int stage = 1; //在哪一个stage了
    public int scene_index; 
    public List<string> scene_list = new List<string>();
    public string mapseed;// 地图种子


    [Header("stageA的地图池")]
    public List<string> stageA_list = new List<string>();

    [Header("safeLevel的地图池")]
    public List<string> saveLevel_list = new List<string>();

    [Header("长期存档内容")]
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
        rogueItems_canChose = new List<GameObject>(rogueItems_ALL);  // 最开始的时候，列表是有全部的内容的
    }

    // Update is called once per frame
    void Update()
    {

        // 测试用。由于还没有目前的流程还不足以完全使用SaveSystem (没有好的时机使用)
        // 我们这里测试以验证其是可行的
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
        scene_list.Clear(); // 每次使用前都clear一次
        if (stage == 1)
        {
            GameManager.Shuffle<string>(stageA_list);
            GameManager.Shuffle<string>(saveLevel_list);
            
            // 按照 012 ->安全屋 ->3->boss的顺序
            scene_list.Add(stageA_list[0]);
            scene_list.Add(stageA_list[1]);
            scene_list.Add(stageA_list[2]);
            scene_list.Add(saveLevel_list[0]);
            scene_list.Add(stageA_list[3]);
            scene_list.Add("TestBoss"); // 这里因为就设置了一个Boss，所以先直接加入。后面按每一层的Boss的名字加入


        }
    }

    public void generateSeed()
    {
        string tmp = "";
        foreach (string sc in scene_list)
        {
            tmp += sc;// 连接 
        }

        Debug.Log(tmp); //得到结果
        mapseed = tmp; // 记录


    }

    /// <summary>
    /// 每一次开始的时候/切换场景的时候，Player都应该向Mgr注册
    /// </summary>
    /// <param name="_player"></param>
    public void getNewPlayer(GameObject _player)
    {
        player = _player;
        playerSC = player.GetComponent<PlayerBase>(); 
    }


    /// <summary>
    /// 更新玩家的得分情况
    /// </summary>
    public void UpdatePlayerScore()
    {
        theScore= 0; //清零

        theScore = playerSC.levelScore;  //更新

        theScore_sum += theScore; 
    }

    // 实现洗牌算法
    // Fisher-Yates洗牌算法
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

        // 加入管理器的，已经选的列表
        rogueItems_chosen.Add(_item);

        // 首先加入玩家的列表
        int tmp = rogueItem.type;

        if(tmp== 0) playerSC.dataItems.Add(_item);
        else if (tmp == 1) playerSC.FireItems.Add(_item);
        else if (tmp == 2) playerSC.AttackItems.Add(_item);
        else if (tmp == 3) playerSC.DashOnItems.Add(_item);
        else if (tmp == 4) playerSC.DashOffItems.Add(_item);
        else if (tmp == 5) playerSC.HurtItems.Add(_item);
        else Debug.Log("ADD ITEM FALSE!!!");

        // 然后从天赋池中删除
        rogueItems_canChose.Remove(_item);

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(scene_list[scene_index]); //
        scene_index++; //更新下标
    }

    public void LoadNextScene(string _name)
    {
        SceneManager.LoadScene(_name); //
    }


    // 存档系统相关

    // ShortSave和LongSave的样式在该部分之后

    // Shortave
    public ShortSave CreateShortSave()
    {
        // 创建一个save对象
        ShortSave save = new ShortSave();

        return save;
    }

    public void ShortSaveByJson()
    {
        //一样的，我们使用CreateSaveGameObject()方法，得到要保存的Save
        ShortSave save = CreateShortSave();

        save.theScore_sum = theScore_sum;
        save.rogueItems_canChose = rogueItems_canChose;
        save.rogueItems_chosen = rogueItems_chosen;
        save.stage = stage;
        save.scene_index = scene_index;
        save.mapseed = mapseed;

    //! Json的保存方式是JsonString，因此我们需要创建一个对应的String
    string JsonString = JsonUtility.ToJson(save);
        //利用 JsonUtility.ToJson()得到我们所需要的东西，【对象save】->【JSON字符串JsonString】

        //随后实例化一个流写入类：StreamWriter类
        StreamWriter sw = new StreamWriter(Application.dataPath + "ShortSave_DATA.txt");
        //StreamWriter类允许将字符和字符串写入文件，参数是写入数据的地方的“完整文件路径”
        sw.Write(JsonString);//写入
        sw.Close();

        Debug.Log("FINSH: " + Application.dataPath); 
    }

    public void LoadShortSave()
    {
        // 检测文件存在
        if (File.Exists(Application.dataPath + "ShortSave_DATA.txt"))
        {
            //读取文件
            StreamReader sr = new StreamReader(Application.dataPath + "ShortSave_DATA.txt");


            //将文件转换为string
            string JsonString = sr.ReadToEnd();            
            //ReadToEnd()读取的是来自流的当前位置到结尾的所有字符(从整个流或流的当前位置，读取至结尾)
            sr.Close();

            // 得到save文件
            ShortSave save = JsonUtility.FromJson<ShortSave>(JsonString);

            //按需求还原

            theScore_sum = save.theScore_sum;
            rogueItems_canChose = save.rogueItems_canChose;
            rogueItems_chosen = save.rogueItems_chosen;
            stage = save.stage;
            scene_index = save.scene_index;
            mapseed = save.mapseed;
        }
        else
        {
            // 不存在的话。由于这个是短期存档。所以可以先不作处理
            Debug.Log("暂无ShortSave");
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
        //一样的，我们使用CreateSaveGameObject()方法，得到要保存的Save
        LongSave save = CreateLongSave();

        save.gameTime= gameTime+1; //现在是保存的时候都默认加一次

        //! Json的保存方式是JsonString，因此我们需要创建一个对应的String
        string JsonString = JsonUtility.ToJson(save);
        //利用 JsonUtility.ToJson()得到我们所需要的东西，【对象save】->【JSON字符串JsonString】

        //随后实例化一个流写入类：StreamWriter类
        StreamWriter sw = new StreamWriter(Application.dataPath + "LongSave_DATA.txt");
        //StreamWriter类允许将字符和字符串写入文件，参数是写入数据的地方的“完整文件路径”
        sw.Write(JsonString);//写入
        sw.Close();
    }

    public void LoadLongSave()
    {
        // 检测文件存在
        if (File.Exists(Application.dataPath + "LongSave_DATA.txt"))
        {
            //读取文件
            StreamReader sr = new StreamReader(Application.dataPath + "LongSave_DATA.txt");


            //将文件转换为string
            string JsonString = sr.ReadToEnd();
            //ReadToEnd()读取的是来自流的当前位置到结尾的所有字符(从整个流或流的当前位置，读取至结尾)
            sr.Close();

            // 得到save文件
            LongSave save = JsonUtility.FromJson<LongSave>(JsonString);

            //按需求还原
            gameTime = save.gameTime;
        }
        else
        {
            // 不存在的话。这个是LongSave。则意味着需要作用下初始化处理
            gameTime = 1; 
        }
    }


    // 延迟执行
    public void DelayedFunction(float delay, System.Action function)
    {
        StartCoroutine(DelayedCoroutine(delay, function));
    }

    private IEnumerator DelayedCoroutine(float delay, System.Action function)
    {
        yield return new WaitForSeconds(delay);

        // 在这里执行延迟函数
        function?.Invoke();
    }

}


[System.Serializable]
public class LongSave
{
    //仅做演示，目前longSave还没有很充分的保存内容。这里只存储了游戏次数作为展示使用

    public int gameTime; 
}
