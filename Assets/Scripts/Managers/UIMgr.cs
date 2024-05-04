using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingletonMono<UIMgr>
{
    [Header("数据对象区")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;



    [Header("UI组件区")]
    public GameObject MainUI_Playing; 
    public Scrollbar hpBar;
    public Scrollbar scoreBar;
    public Scrollbar RhythmScoreBar;
    public TextMeshProUGUI beatValue ;
    public TextMeshProUGUI AttackUpNum ;
    public Image DashCD;

    public Image RhyBar;



    private void Start()
    {
        MainUI_Playing = GameObject.Find("MainUI_Playing");

        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();

        hpBar = MainUI_Playing.transform.Find("HealthBar").GetComponent<Scrollbar>();
        RhythmScoreBar = MainUI_Playing.transform.Find("RhythmBar").GetComponent<Scrollbar>();

        beatValue = MainUI_Playing.transform.Find("BeatValue").GetComponent<TextMeshProUGUI>();
        AttackUpNum = MainUI_Playing.transform.Find("AtackUpNum").GetComponent<TextMeshProUGUI>();

        DashCD = MainUI_Playing.transform.Find("DashCD").GetComponent<Image>();
        RhyBar = MainUI_Playing.transform.Find("OrangeImg").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 对于Scrollbar调整的是size
        hpBar.size = PlayerSc.nowHp / PlayerSc.maxHp;

        RhythmScoreBar.size = PlayerSc.nowBeatValue / (100.0f); // 最大值反正是100

        DashCD.fillAmount = PlayerSc.dashTimer / PlayerSc.dashCD;

        RhyBar.fillAmount = (1.0f - ((RhythmMgr.GetInstance().gettimeToArrive()) /
            (RhythmMgr.GetInstance().getdelayPlay_Record())));

        UpdateBeatBalue(PlayerSc.levelScore);

    }
    public void UpdateBeatBalue(int _score)
    {
        string tem = _score.ToString()+"  ";
        string oth = " "; 

        if(_score < 30)
        {
            oth = " O _ O ";

        }

        if (_score > 30)
        {
            oth = " Q v Q ";
        }
        
        if(_score > 50)
        {
            oth = "O w O ";

        }
        
        if(_score > 100)
        {
            oth = "> w < ";

        }


        beatValue.text = tem + oth;

        AttackUpNum.text = PlayerSc.attackUpLevel.ToString(); 
    }
}
