using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingletonMono<UIMgr>
{
    [Header("���ݶ�����")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;



    [Header("UI�����")]
    public Scrollbar hpBar;
    public Scrollbar beatBar;
    public Image DashCD; 


    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
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
        beatBar.size = PlayerSc.nowBeatValue / (100.0f); // ���ֵ������100
        DashCD.fillAmount = PlayerSc.dashTimer / PlayerSc.dashCD; 

    }
}
