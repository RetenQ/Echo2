using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingletonMono<UIMgr>
{
    [Header("���ݶ�����")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;



    [Header("UI�����")]
    public GameObject MainUI_Playing; 
    public Scrollbar hpBar;
    public Scrollbar RhythmBar;
    public Image DashCD;

    public Image RhyBar;



    private void Start()
    {
        MainUI_Playing = GameObject.Find("MainUI_Playing");

        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();

        hpBar = MainUI_Playing.transform.Find("HealthBar").GetComponent<Scrollbar>();
        RhythmBar = MainUI_Playing.transform.Find("RhythmBar").GetComponent<Scrollbar>();
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
        // ����Scrollbar��������size
        hpBar.size = PlayerSc.nowHp / PlayerSc.maxHp;
        RhythmBar.size = PlayerSc.nowBeatValue / (100.0f); // ���ֵ������100
        DashCD.fillAmount = PlayerSc.dashTimer / PlayerSc.dashCD;

        RhyBar.fillAmount = (1.0f - ((RhythmMgr.GetInstance().gettimeToArrive()) /
            (RhythmMgr.GetInstance().getdelayPlay_Record())));


    }
}
