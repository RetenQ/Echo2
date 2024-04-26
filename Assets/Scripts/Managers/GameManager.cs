using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
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



    // Start is called before the first frame update
    void Start()
    {
        rogueItems_canChose = new List<GameObject>(rogueItems_ALL);  // 最开始的时候，列表是有全部的内容的
    }

    // Update is called once per frame
    void Update()
    {
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
        playerSC = playerSC.GetComponent<PlayerBase>(); 
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

        if (tmp == 1) playerSC.FireItems.Add(_item);
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



}
