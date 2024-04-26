using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{

    [Header("������ѡȡ�ĵ��ߴ洢")]
    public List<GameObject> rogueItems = new List<GameObject>();

    [Header("�Ѿ�ѡȡ�ĵ��ߴ洢")]
    public List<GameObject> rogueItems_chosen = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void Set3RoughtItem()
    {
        
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




}
